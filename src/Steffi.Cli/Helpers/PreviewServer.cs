using Spectre.Console;
using Steffi.Parsers;
using Steffi.Renderers.Svg;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Steffi.Cli.Helpers;

internal class PreviewServer : IDisposable
{
	private readonly string _inputFilePath;
	private readonly string _fileName;
	private readonly string _interactiveUrl;

	private FileSystemWatcher? _fileWatcher;

	private readonly string _tempSvgPath;
	private readonly string _tempErrorPath;

	private readonly HttpListener _listener;
	private readonly CancellationTokenSource _shutdownCts = new();
	private ConcurrentBag<HttpListenerResponse> _clients = new();

	private readonly SteffiParser _parser = new();
	private byte[] _lastParsingHash = [];

	private bool _disposed;

	public PreviewServer(string inputFilePath, int port)
	{
		_inputFilePath = Path.GetFullPath(inputFilePath);
		_fileName = Path.GetFileName(_inputFilePath);
		_interactiveUrl = $"http://localhost:{port}/";
		_tempSvgPath = Path.Combine(Path.GetTempPath(), $"steffi-preview-{Guid.NewGuid()}.svg");
		_tempErrorPath = Path.ChangeExtension(_tempSvgPath, ".errors");
		_listener = new HttpListener();
		_listener.Prefixes.Add($"http://localhost:{port}/");
	}

	public async Task<int> StartAsync()
	{
		_listener.Start();

		Console.CancelKeyPress += (_, e) => { e.Cancel = true; _shutdownCts.Cancel(); };

		AnsiConsole.WriteLine();
		AnsiConsole.MarkupLine("[cyan bold]═══════════════════════════════════════════════════════[/]");
		AnsiConsole.MarkupLine("[cyan bold]  Steffi Interactive Preview Started[/]");
		AnsiConsole.MarkupLine("[cyan bold]═══════════════════════════════════════════════════════[/]");
		AnsiConsole.MarkupLine($"  [green]File:[/] {Markup.Escape(_inputFilePath)}");
		AnsiConsole.MarkupLine($"  [green]URL:[/] [link]{_interactiveUrl}[/]");
		AnsiConsole.MarkupLine("[cyan bold]═══════════════════════════════════════════════════════[/]");
		AnsiConsole.MarkupLine("[dim]Watching for changes... (Press Ctrl+C to stop)[/]");
		AnsiConsole.WriteLine();

		string content = await TryToGetInputFileContent();
		await GenerateSvgOrErrorsAsync(content);
		SetupFileWatcher();

		TryOpenLocalBrowser(_interactiveUrl);

		try
		{
			await HandleRequestsAsync(_shutdownCts.Token);
		}
		catch (OperationCanceledException) { }
		finally
		{
			AnsiConsole.MarkupLine("[dim]Stopped[/]");
			_listener.Stop();
		}

		return 0;
	}

	private async Task HandleRequestsAsync(CancellationToken token)
	{
		token.Register(() => _listener.Stop());

		while (true)
		{
			HttpListenerContext context;
			try
			{
				context = await _listener.GetContextAsync();
			}
			catch (HttpListenerException)
			{
				break;
			}

			_ = Task.Run(() => RouteRequest(context, token), token);
		}
	}

	private async Task RouteRequest(HttpListenerContext context, CancellationToken token)
	{
		var path = context.Request.Url?.AbsolutePath ?? "/";

		try
		{
			if (path == "/" || path.StartsWith("/interactive", StringComparison.Ordinal))
			{
				await ServeInteractivePreview(context.Response);
			}
			else if (path == "/svg")
			{
				await ServeGeneratedSvgFile(context.Response);
			}
			else if (path == "/events")
			{
				await ServeGenerationEvents(context.Response, token);
			}
			else
			{
				context.Response.StatusCode = 302;
				context.Response.AddHeader("Location", _interactiveUrl);
				context.Response.Close();
			}
		}
		catch
		{
			try { context.Response.StatusCode = 500; context.Response.Close(); } catch { }
		}
	}

	private async Task<string> TryToGetInputFileContent()
	{
		for (int attempt = 1; attempt <= 5; attempt++)
		{
			try
			{
				return await File.ReadAllTextAsync(_inputFilePath);
			}
			catch (IOException)
			{
				if (attempt == 5)
				{
					throw;
				}
				await Task.Delay(100);
			}
		}

		throw new InvalidOperationException();
	}

	private async Task ServeGeneratedSvgFile(HttpListenerResponse response)
	{
		if (File.Exists(_tempSvgPath))
		{
			response.ContentType = "image/svg+xml";
			var bytes = await File.ReadAllBytesAsync(_tempSvgPath);
			response.ContentLength64 = bytes.Length;
			await response.OutputStream.WriteAsync(bytes);
		}
		else
		{
			response.StatusCode = 404;
		}
		response.Close();
	}

	private async Task ServeGenerationEvents(HttpListenerResponse response, CancellationToken token)
	{
		response.ContentType = "text/event-stream";
		response.Headers["Cache-Control"] = "no-cache";
		response.Headers["X-Accel-Buffering"] = "no";
		response.SendChunked = true;

		_clients.Add(response);

		if (File.Exists(_tempErrorPath))
		{
			var errorsJson = await File.ReadAllTextAsync(_tempErrorPath);
			await WriteEventAsync(response, "error", errorsJson);
		}

		try
		{
			await Task.Delay(Timeout.Infinite, token);
		}
		catch (OperationCanceledException) { }
		finally
		{
			try { response.Close(); } catch { }
		}
	}

	private static async Task ServeInteractivePreview(HttpListenerResponse response)
	{
		var html = """
			<!DOCTYPE html>
			<html>
			<head>
				<meta charset="utf-8">
				<meta name="viewport" content="width=device-width, initial-scale=1">
				<title>Steffi Interactive Preview</title>
				<style>
					* {
						margin: 0;
						padding: 0;
						box-sizing: border-box;
					}
					body {
						width: 100vw;
						height: 100vh;
						overflow: hidden;
						background: #1e1e1e;
						display: flex;
						flex-direction: column;
					}
					#svg-container {
						flex: 1;
						display: flex;
						justify-content: center;
						align-items: center;
						overflow: auto;
						background: #2d2d2d;
					}
					#svg-container svg {
						max-width: 100%;
						max-height: 100%;
					}
					#error-overlay {
						position: fixed;
						top: 0;
						left: 0;
						right: 0;
						background: #c0392b;
						color: white;
						padding: 20px;
						font-family: 'Consolas', 'Monaco', monospace;
						font-size: 14px;
						display: none;
						max-height: 300px;
						overflow: auto;
						box-shadow: 0 2px 10px rgba(0,0,0,0.3);
						z-index: 1000;
					}
					#error-overlay.show {
						display: block;
					}
					#error-overlay h3 {
						margin-bottom: 10px;
						font-size: 16px;
						font-weight: bold;
					}
					#error-overlay ul {
						list-style: none;
						padding-left: 0;
					}
					#error-overlay li {
						padding: 5px 0;
						border-bottom: 1px solid rgba(255,255,255,0.1);
					}
					#error-overlay li:last-child {
						border-bottom: none;
					}
					#status-bar {
						background: #1a1a1a;
						color: #888;
						padding: 8px 16px;
						font-family: 'Consolas', 'Monaco', monospace;
						font-size: 12px;
						border-top: 1px solid #333;
						display: flex;
						justify-content: space-between;
						align-items: center;
					}
					.status-indicator {
						display: inline-block;
						width: 8px;
						height: 8px;
						border-radius: 50%;
						margin-right: 6px;
					}
					.status-indicator.connected {
						background: #27ae60;
					}
					.status-indicator.disconnected {
						background: #c0392b;
					}
					.status-indicator.reloading {
						background: #f39c12;
					}
				</style>
			</head>
			<body>
				<div id="error-overlay"></div>
				<div id="svg-container"></div>
				<div id="status-bar">
					<span>
						<span class="status-indicator connected" id="status-indicator"></span>
						<span id="status-text">Connected</span>
					</span>
					<span id="file-info">Steffi Interactive Preview</span>
				</div>
				<script>
					const container = document.getElementById('svg-container');
					const errorOverlay = document.getElementById('error-overlay');
					const statusIndicator = document.getElementById('status-indicator');
					const statusText = document.getElementById('status-text');

					function setStatus(status, text) {
						statusIndicator.className = 'status-indicator ' + status;
						statusText.textContent = text;
					}

					async function loadSvg() {
						try {
							setStatus('reloading', 'Loading...');
							const response = await fetch('/svg');
							const svg = await response.text();
							container.innerHTML = svg;
							errorOverlay.classList.remove('show');
							setStatus('connected', 'Connected');
							console.log('SVG loaded successfully');
						} catch (error) {
							console.error('Failed to load SVG:', error);
							setStatus('disconnected', 'Error loading SVG');
						}
					}

					function showErrors(errors) {
						errorOverlay.innerHTML = '<h3>⚠️ Parsing Errors</h3><ul>' +
							errors.map(e => '<li>' + e + '</li>').join('') +
							'</ul>';
						errorOverlay.classList.add('show');
						setStatus('disconnected', 'Parse Error');
					}

					// Initial load
					loadSvg();

					// Setup Server-Sent Events
					const eventSource = new EventSource('/events');

					eventSource.onopen = () => {
						console.log('SSE connected');
						setStatus('connected', 'Connected');
					};

					eventSource.onerror = () => {
						console.error('SSE connection error');
						setStatus('disconnected', 'Disconnected');
					};

					eventSource.addEventListener('reload', (event) => {
						console.log('Reload event received');
						loadSvg();
					});

					eventSource.addEventListener('error', (event) => {
						const errors = JSON.parse(event.data);
						console.error('Parse errors:', errors);
						showErrors(errors);
					});
				</script>
			</body>
			</html>
			""";

		response.ContentType = "text/html; charset=utf-8";
		var bytes = Encoding.UTF8.GetBytes(html);
		response.ContentLength64 = bytes.Length;
		await response.OutputStream.WriteAsync(bytes);
		response.Close();
	}

	private async void OnInputFileChanged(object sender, FileSystemEventArgs e)
	{
		var fileName = Path.GetFileName(_inputFilePath);

		if (!string.Equals(e.Name, fileName, StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		await Task.Delay(100);

		string content = await TryToGetInputFileContent();

		var newHash = SHA256.HashData(Encoding.UTF8.GetBytes(content));
		if (newHash.SequenceEqual(_lastParsingHash))
		{
			return;
		}

		AnsiConsole.MarkupLine($"[dim][[{DateTime.Now:HH:mm:ss}]][/] File changed: {Markup.Escape(e.Name ?? "")}");

		if (await GenerateSvgOrErrorsAsync(content))
		{
			await NotifyClientsAsync("reload", "");
			AnsiConsole.MarkupLine($"[dim][[{DateTime.Now:HH:mm:ss}]][/] [green]✓ Reloaded[/]");
		}
	}

	private void SetupFileWatcher()
	{
		var directory = Path.GetDirectoryName(_inputFilePath)!;
		var fileName = Path.GetFileName(_inputFilePath);

		_fileWatcher = new FileSystemWatcher(directory, fileName);
		_fileWatcher.Changed += OnInputFileChanged;
		_fileWatcher.Created += OnInputFileChanged;
		_fileWatcher.Renamed += async (sender, e) => OnInputFileChanged(sender, e);
		_fileWatcher.EnableRaisingEvents = true;
	}

	private async Task<bool> GenerateSvgOrErrorsAsync(string content)
	{
		try
		{
			if (!File.Exists(_inputFilePath))
			{
				AnsiConsole.MarkupLine("[red]File not found:[/] " + Markup.Escape(_inputFilePath));
				return false;
			}

			var (document, errors) = _parser.Parse(content);
			_lastParsingHash = SHA256.HashData(Encoding.UTF8.GetBytes(content));

			if (errors.Count > 0)
			{
				AnsiConsole.MarkupLine("[red]Parsing failed:[/]");
				foreach (var error in errors)
				{
					AnsiConsole.MarkupLine($"[red]- {Markup.Escape(error)}[/]");
				}

				// Notify clients about errors
				var errorsJson = JsonSerializer.Serialize(errors, PreviewServerJsonContext.Default.ListString);
				await NotifyClientsAsync("error", errorsJson);

				await File.WriteAllTextAsync(_tempErrorPath, errorsJson);

				return false;
			}

			FileHelper.TryDeleteFileIfExists(_tempErrorPath);

			var renderer = new SvgRenderer();
			var svgContent = renderer.RenderDocument(document!);

			await File.WriteAllTextAsync(_tempSvgPath, svgContent);

			return true;
		}
		catch (Exception ex)
		{
			AnsiConsole.MarkupLine($"[red]Error generating SVG:[/] {Markup.Escape(ex.Message)}");
			return false;
		}
	}

	private async Task NotifyClientsAsync(string eventType, string data)
	{
		var disconnectedClients = new List<HttpListenerResponse>();

		foreach (var client in _clients)
		{
			try
			{
				await WriteEventAsync(client, eventType, data);
			}
			catch
			{
				disconnectedClients.Add(client);
			}
		}

		if (disconnectedClients.Count > 0)
		{
			Interlocked.Exchange(ref _clients, [.. _clients.Except(disconnectedClients)]);
		}
	}

	private static async Task WriteEventAsync(HttpListenerResponse response, string eventType, string data)
	{
		var bytes = Encoding.UTF8.GetBytes($"event: {eventType}\ndata: {data}\n\n");
		await response.OutputStream.WriteAsync(bytes);
		await response.OutputStream.FlushAsync();
	}

	private void TryOpenLocalBrowser(string url)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = url,
				UseShellExecute = true
			});
		}
		catch
		{
			AnsiConsole.MarkupLine($"[red]Error opening browser:[/]");
		}
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		_shutdownCts.Cancel();
		_fileWatcher?.Dispose();
		_shutdownCts.Dispose();

		FileHelper.TryDeleteFileIfExists(_tempErrorPath);
		FileHelper.TryDeleteFileIfExists(_tempSvgPath);
	}
}
