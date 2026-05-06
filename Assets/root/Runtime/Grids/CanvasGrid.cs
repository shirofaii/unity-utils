using System.Runtime.CompilerServices;
using UnityEngine;
using static Globals;

namespace SharedUtils {
public class CanvasGrid {
    private Vector2Int margins = new Vector2Int(18, 20);
    private Vector2Int cellSize = new Vector2Int(39, 37);
    public readonly Vector2Int gridSize = new Vector2Int(42, 24);
    private float scaleFactor = 1f;
    public readonly int gap = 6;
    
    public Vector2 worldUnitsInCamera;
    public Vector2 pixelToWorld;

    private readonly Rect[] cells;
    private readonly Rect[] cellsWorldsSpace;
    private Vector2Int screenSize;

    public CanvasGrid() {
        cells = new Rect[gridSize.x * gridSize.y];
        cellsWorldsSpace = new Rect[gridSize.x * gridSize.y];
        Recalculate();
    }

    [MethodImpl(inline)] public Rect GetCellScreenSpace(int x, int y) {
        return cells[x * gridSize.y + y];
    }

    [MethodImpl(inline)] public Rect GetCellScreenSpace(Vector2Int position) {
        return cells[position.x * gridSize.y + position.y];
    }

    [MethodImpl(inline)] public Rect GetCellWorldSpace(int x, int y) {
        return cellsWorldsSpace[x * gridSize.y + y];
    }

    [MethodImpl(inline)] public Rect GetCellWorldSpace(Vector2Int position) {
        return cellsWorldsSpace[position.x * gridSize.y + position.y];
    }

    [MethodImpl(inline)] public Rect GetRectSizeScreenSpace(RectInt rect) {
        var min = cells[rect.min.x * gridSize.y + rect.min.y].min;
        var max = cells[(rect.max.x - 1) * gridSize.y + (rect.max.y - 1)].max;
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    [MethodImpl(inline)] public Rect GetRectSizeWorldSpace(RectInt rect) {
        var min = cellsWorldsSpace[rect.min.x * gridSize.y + rect.min.y].min;
        var max = cellsWorldsSpace[(rect.max.x - 1) * gridSize.y + (rect.max.y - 1)].max;
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    [MethodImpl(inline)] public void Update() {
        if(screenSize.x == Screen.width && screenSize.y == Screen.height) {
            return;
        }

        screenSize = new Vector2Int(Screen.width, Screen.height);
        Recalculate();
    }

    private void Recalculate() {
        var cam = Camera.main!;

        worldUnitsInCamera.y = cam.orthographicSize * 2;
        worldUnitsInCamera.x = worldUnitsInCamera.y * Screen.width / Screen.height;

        pixelToWorld.x = worldUnitsInCamera.x / Screen.width;
        pixelToWorld.y = worldUnitsInCamera.y / Screen.height;
        
        var referenceResolution = new Vector2(1920, 1080);
        scaleFactor = Mathf.Min(screenSize.x / referenceResolution.x, screenSize.y / referenceResolution.y);
        var scaledMargins = new Vector2(margins.x * scaleFactor, margins.y * scaleFactor);
        var scaledCellSize = new Vector2(cellSize.x * scaleFactor, cellSize.y * scaleFactor);
        var scaledGap = gap * scaleFactor;

        var step = scaledCellSize + new Vector2(scaledGap, scaledGap);
        var full = scaledMargins * 2 + step * gridSize;
        scaledMargins += (screenSize - full) * 0.5f;

        for(var x = 0; x < gridSize.x; x++) {
            for(var y = 0; y < gridSize.y; y++) {
                var minPos = new Vector2(scaledMargins.x + x * step.x, scaledMargins.y + y * step.y);
                cells[x * gridSize.y + y] = new Rect(minPos, scaledCellSize);

                var wMinPos = cam.ScreenToWorldPoint(minPos);
                var wMaxPos = cam.ScreenToWorldPoint(minPos + scaledCellSize);
                cellsWorldsSpace[x * gridSize.y + y] = Rect.MinMaxRect(wMinPos.x, wMinPos.y, wMaxPos.x, wMaxPos.y);
            }
        }
    }
}
}