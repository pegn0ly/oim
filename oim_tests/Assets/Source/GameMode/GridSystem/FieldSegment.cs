using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private int _xCenter { get; }
    private int _yCenter { get; }
    private int _GridPos { get; }
    public Cell(int x_coord, int y_coord, int grid_position)
    {
        _xCenter = x_coord;
        _yCenter = y_coord;
        _GridPos = grid_position;
    }
}

public class FieldSegment : MonoBehaviour
{
    // клетки, определ€ющие данный сегмент пол€.
    private List<Cell> Cells;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCell(Cell cell_to_add)
    {
        Cells.Add(cell_to_add);
    }
}
