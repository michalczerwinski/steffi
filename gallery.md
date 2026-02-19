# Steffi Gallery

A showcase of images created with the Steffi DSL. Each example demonstrates different language features — shapes, layouts, colors, and composition.

> **Regenerate all SVGs** by running:
> ```bash
> .\scripts\Regenerate-Gallery.ps1
> ```

---

## Snowman

A classic snowman built with stacked ellipses and circles. Demonstrates `VerticalStack` with `spacing: 0` to snap body sections together, and absolute positioning inside `Canvas` for facial features.

<table>
<tr>
<td>

```scss
VerticalStack {
  spacing: 0;
  Canvas {
    Ellipse { x: 16; y: 0; rx: 20; ry: 18;
      strokeWidth: 2; stroke: "slategray"; fill: "white"; }
    Circle { x: 27; y: 10; r: 3; fill: "black"; }
    Circle { x: 39; y: 10; r: 3; fill: "black"; }
    Ellipse { x: 32; y: 22; rx: 4; ry: 2;
      fill: "orange"; stroke: "darkorange"; }
  }
  Canvas {
    Ellipse { x: 8; y: 0; rx: 28; ry: 26;
      strokeWidth: 2; stroke: "slategray"; fill: "white"; }
    Circle { x: 33; y: 18; r: 3; fill: "dimgray"; }
    Circle { x: 33; y: 30; r: 3; fill: "dimgray"; }
  }
  Canvas {
    Ellipse { x: 0; y: 0; rx: 36; ry: 32;
      strokeWidth: 2; stroke: "slategray"; fill: "white"; }
    Circle { x: 33; y: 16; r: 3; fill: "dimgray"; }
    Circle { x: 33; y: 28; r: 3; fill: "dimgray"; }
    Circle { x: 33; y: 40; r: 3; fill: "dimgray"; }
  }
}
```

</td>
<td>

![Snowman](gallery/snowman.svg)

</td>
</tr>
</table>

**Source:** [`gallery/snowman.stf`](gallery/snowman.stf)

---

## Color Palette

A grid of named CSS colors arranged with horizontal and vertical stacks. Shows `spacing` for gaps and `rx`/`ry` for rounded corners.

<table>
<tr>
<td>

```scss
VerticalStack {
  spacing: 4;
  Text { spans: "Color Palette"; fontSize: 18;
    fontColor: "dimgray"; }
  HorizontalStack {
    spacing: 4;
    Rectangle { width: 50; height: 50;
      fill: "tomato"; rx: 4; ry: 4; }
    Rectangle { width: 50; height: 50;
      fill: "coral"; rx: 4; ry: 4; }
    Rectangle { width: 50; height: 50;
      fill: "gold"; rx: 4; ry: 4; }
    Rectangle { width: 50; height: 50;
      fill: "orange"; rx: 4; ry: 4; }
    Rectangle { width: 50; height: 50;
      fill: "salmon"; rx: 4; ry: 4; }
  }
  // … green, blue, and pink rows follow
}
```

</td>
<td>

![Color Palette](gallery/color-palette.svg)

</td>
</tr>
</table>

**Source:** [`gallery/color-palette.stf`](gallery/color-palette.stf)

---

## Abstract Shapes

Overlapping circles, rectangles, and ellipses with semi-transparent fills. Demonstrates `fillOpacity` and layered `Canvas` composition.

<table>
<tr>
<td>

```scss
Canvas {
  Circle { x: 80; y: 80; r: 70;
    fill: "dodgerblue"; fillOpacity: "0.4";
    stroke: "royalblue"; strokeWidth: 2; }
  Circle { x: 140; y: 80; r: 70;
    fill: "tomato"; fillOpacity: "0.4";
    stroke: "firebrick"; strokeWidth: 2; }
  Circle { x: 110; y: 140; r: 70;
    fill: "gold"; fillOpacity: "0.4";
    stroke: "goldenrod"; strokeWidth: 2; }
  Rectangle { x: 40; y: 40; width: 60; height: 60;
    fill: "mediumseagreen"; fillOpacity: "0.5";
    rx: 10; ry: 10; stroke: "darkgreen"; }
  Rectangle { x: 160; y: 160; width: 60; height: 60;
    fill: "orchid"; fillOpacity: "0.5";
    rx: 10; ry: 10; stroke: "purple"; }
  Ellipse { x: 110; y: 110; rx: 100; ry: 50;
    fill: "none"; stroke: "slategray"; strokeWidth: 3; }
}
```

</td>
<td>

![Abstract Shapes](gallery/abstract-shapes.svg)

</td>
</tr>
</table>

**Source:** [`gallery/abstract-shapes.stf`](gallery/abstract-shapes.stf)

---

## Dashboard

A mock dashboard with stat cards and a bar chart. Demonstrates nested `VerticalStack`/`HorizontalStack` layouts, `padding`, container `fill`/`stroke`, and `Text` styling.

<table>
<tr>
<td>

```scss
VerticalStack {
  spacing: 8; padding: 12;
  Text { spans: "Dashboard"; fontSize: 22;
    fontColor: "dimgray"; }
  HorizontalStack {
    spacing: 8;
    VerticalStack {
      padding: 10; fill: "lavender";
      stroke: "slateblue"; strokeWidth: 1;
      Text { spans: "Users"; fontSize: 12; }
      Text { spans: "1,247"; fontSize: 28;
        fontColor: "navy"; }
      Rectangle { width: 100; height: 6;
        fill: "slateblue"; rx: 3; ry: 3; }
    }
    // … Revenue and Errors cards follow
  }
  HorizontalStack {
    spacing: 4;
    Rectangle { width: 30; height: 80;
      fill: "slateblue"; rx: 2; ry: 2; }
    Rectangle { width: 30; height: 120;
      fill: "slateblue"; rx: 2; ry: 2; }
    // … more bars follow
  }
}
```

</td>
<td>

![Dashboard](gallery/dashboard.svg)

</td>
</tr>
</table>

**Source:** [`gallery/dashboard.stf`](gallery/dashboard.stf)

---

## Bullseye

Concentric circles forming a classic target. Showcases layered `Canvas` elements where each circle's `x`/`y` is offset to keep all centers aligned.

<table>
<tr>
<td>

```scss
Canvas {
  Circle { x: 0; y: 0; r: 110;
    fill: "firebrick"; stroke: "darkred"; }
  Circle { x: 20; y: 20; r: 90;
    fill: "white"; stroke: "darkred"; }
  Circle { x: 40; y: 40; r: 70;
    fill: "firebrick"; stroke: "darkred"; }
  Circle { x: 60; y: 60; r: 50;
    fill: "white"; stroke: "darkred"; }
  Circle { x: 80; y: 80; r: 30;
    fill: "firebrick"; stroke: "darkred"; }
  Circle { x: 100; y: 100; r: 10;
    fill: "gold"; stroke: "goldenrod"; }
}
```

</td>
<td>

![Bullseye](gallery/bullseye.svg)

</td>
</tr>
</table>

**Source:** [`gallery/bullseye.stf`](gallery/bullseye.stf)
