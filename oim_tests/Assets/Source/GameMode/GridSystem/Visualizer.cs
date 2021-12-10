using System;
using System.Collections.Generic;
using UnityEngine;

namespace OIMField
{
    // класс, отвечающий за визуализацию сетки боя
    
    // Поля:
    // - Sprite - ссылка на ресурс спрайта клетки
    // - TileMap - информация о положении спрайтов на сетке
    // - CurrentlyHighlightedSprited - спрайт, подсвеченный в данный момент(при наведении)
    // - GridToVisualize - ссылка на сетку, которую нужно отрисовать
    // - CellColors - цвета спрайта, привязанные к разным состояниям клетки
    public class FieldVisualizer : MonoBehaviour 
    {
        private GameObject Sprite;
        private SortedList<int, SpriteRenderer> TileMap = new SortedList<int, SpriteRenderer>();
        private int CurrentlyHighlightedSprite = -1000;
        private CellGrid GridToVisualize;
        private struct CellColors
        {
            public static Color Default = Color.black;
            public static Color Highlighted = Color.green;
            public static Color PathHighlighted = Color.red;
        }

        private void Awake()
        {
            Sprite = Resources.Load("GridSprite") as GameObject;
            transform.SetParent(transform.parent);
            //
            GridToVisualize = (CellGrid)gameObject.GetComponent(typeof(CellGrid));
            //
            ClickDetector.OnHovered += HighlightSprite;
        }

        public void Render(GridProps props)
        {
            for (int i = 0; i < props.Width; i++)
            {
                for (int j = 0; j < props.Height; j++)
                {
                    GameObject NewSprite = Instantiate(Sprite);
                    NewSprite.name = "sprite_" + (j * props.Width + i); 
                    NewSprite.transform.position = new Vector3((props.BaseX + CellGrid.CellSize * i) + CellGrid.CellSize / 2, 0.01f, (props.BaseY + CellGrid.CellSize * j) + CellGrid.CellSize / 2);
                    NewSprite.transform.SetParent(transform);
                    TileMap.Add(j * props.Width + i, NewSprite.GetComponent<SpriteRenderer>());
                }
            }
        }

        private void HighlightSprite(Vector3 point)
        {
            int SpriteCell = GridToVisualize.GetClosestPointToPosition(point);
            if(GridToVisualize.IsValidCell(SpriteCell) && GridToVisualize.IsPassableCell(SpriteCell))
            {
                if(CurrentlyHighlightedSprite != -1000)
                {
                    TileMap[CurrentlyHighlightedSprite].color = CellColors.Default;
                }
                TileMap[SpriteCell].color = CellColors.Highlighted;
                CurrentlyHighlightedSprite = SpriteCell;
            }
            else if(CurrentlyHighlightedSprite != -1000)
            {
                TileMap[CurrentlyHighlightedSprite].color = CellColors.Default;
                CurrentlyHighlightedSprite = -1000;
            }
        }
    }
}
