using UnityEngine;

namespace OIMField
{   
    // класс, распознающий клики мышкой по полю
    
    // Поля:
    // - Cam - камера, относительно которой координаты экрана переводятся в мировые координаты

    // Ивенты:
    // - OnClicked - вызывается при нажатии мышкой на поле, передается позиция курсора на экране
    // - OnHovered - вызывается при наведении мышки на поле, передаются мировые координаты курсора
    public class ClickDetector : MonoBehaviour 
    {
        private Camera Cam;
        public delegate void OnClickDelegate(Vector3 point);
        public static event OnClickDelegate OnClicked;        

        public delegate void OnOverDelegate(Vector3 point);
        public static event OnOverDelegate OnHovered;

        private void Awake()
        {
            Cam = Camera.main;
        }
        private void OnMouseOver()
        {            
            RaycastHit Hit;
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Hit))
            {
                OnHovered(Hit.point);
            }
        }
        private void OnMouseDown()
        {
            if(OnClicked != null)
            {
                OnClicked(Input.mousePosition);
            }
        }
    }
}