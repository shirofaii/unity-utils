using Sirenix.OdinInspector;
using System.Runtime.CompilerServices;
using Scellecs.Morpeh;
using UnityEngine;

public class CanvasGrid {
    private Vector2Int margins = new Vector2Int(18, 20);
    private Vector2Int cellSize = new Vector2Int(39, 37);
    private readonly Vector2Int gridSize = new Vector2Int(42,24);
    private float scaleFactor = 1f;
    private readonly int space = 6;

    private readonly Rect[] cells;
    private readonly Rect[] cellsWorldsSpace;
    private Entity[] entities; 
    private Vector2Int screenSize;

    public CanvasGrid() {
        cells = new Rect[gridSize.x * gridSize.y];
        cellsWorldsSpace = new Rect[gridSize.x * gridSize.y];
        Recalculate();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect GetCell(int x, int y) {
        return cells[x * gridSize.y + y];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect GetCell(Vector2Int position) {
        return cells[position.x * gridSize.y + position.y];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect GetCellWorldSpace(int x, int y) {
        return cellsWorldsSpace[x * gridSize.y + y];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect GetCellWorldSpace(Vector2Int position) {
        return cellsWorldsSpace[position.x * gridSize.y + position.y];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect GetRectSize(RectInt rect) {
        var min = cells[rect.min.x * gridSize.y + rect.min.y].min;
        var max = cells[rect.max.x * gridSize.y + rect.max.y].max;
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y); 
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rect GetRectSizeWorldSpace(RectInt rect) {
        var min = cellsWorldsSpace[rect.min.x * gridSize.y + rect.min.y].min;
        var max = cellsWorldsSpace[rect.max.x * gridSize.y + rect.max.y].max;
        return Rect.MinMaxRect(min.x, min.y, max.x, max.y); 
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity GetCellContent(int x, int y) {
        return entities[x * gridSize.y + y];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity GetCellContent(Vector2Int position) {
        return entities[position.x * gridSize.y + position.y];
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasContent(int x, int y) {
        return entities[x * gridSize.y + y] != default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasContent(Vector2Int position) {
        return entities[position.x * gridSize.y + position.y] != default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasContent(RectInt rect) {
        for (var x = rect.xMin; x <= rect.xMax; x++) {
            for (var y = rect.yMin; y <= rect.yMax; y++) {
                if (entities[x * gridSize.y + y] != default) return true;
            }
        }
        return false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetCellContent(Entity entity, RectInt rect) {
        for (var x = rect.xMin; x <= rect.xMax; x++) {
            for (var y = rect.yMin; y <= rect.yMax; y++) {
                Debug.Assert(entities[x * gridSize.y + y] == default);
                entities[x * gridSize.y + y] = entity;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveContent(RectInt rect) {
        for (var x = rect.xMin; x <= rect.xMax; x++) {
            for (var y = rect.yMin; y <= rect.yMax; y++) {
                Debug.Assert(entities[x * gridSize.y + y] == default);
                entities[x * gridSize.y + y] = default;
            }
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update() {
        if(screenSize.x == Screen.width && screenSize.y == Screen.height) {
            return;
        }
        
        screenSize = new Vector2Int(Screen.width, Screen.height);
        Recalculate();
    }

    [Button]
    private void Recalculate() {
        var cam = Camera.main!;

        var referenceResolution = new Vector2(1920, 1080);
        scaleFactor = Mathf.Min(screenSize.x / referenceResolution.x, screenSize.y / referenceResolution.y);
        var scaledMargins = new Vector2(margins.x * scaleFactor, margins.y * scaleFactor);
        var scaledCellSize = new Vector2(cellSize.x * scaleFactor, cellSize.y * scaleFactor);
        var scaledSpace = space * scaleFactor;
        
        var step = scaledCellSize + new Vector2(scaledSpace, scaledSpace);
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

    
    
    // public override void DrawShapes(Camera cam){
    //     using(Draw.Command(cam)) {
    //         var color = Color.white;
    //         color.a = 0.2f;
    //         Draw.Color = color;
    //         
    //         Draw.LineGeometry = LineGeometry.Flat2D;
    //         Draw.ThicknessSpace = ThicknessSpace.Pixels;
    //         Draw.Thickness = 1;
    //
    //         for(var x = 0; x < gridSize.x; x++) {
    //             for(var y = 0; y < gridSize.y; y++) {
    //                 var rect = GetCellWorldSpace(x, y);
    //                 
    //                 Draw.RectangleBorder(rect, 1);
    //             }
    //         }
    //     }
    // }
}

