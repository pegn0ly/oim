using UnityEngine;

using OIMWeb;

namespace OIMField
{
    // класс должен получить данные о поле из бэкенда и сгенерировать сетку на их основе
    public class FieldGenerator : MonoBehaviour
    {
        public delegate void OnFieldGenerated(GridProps props);
        public static event OnFieldGenerated FieldGenerated;

        // загрузка шаблона поля для генерации
        public void StartGeneration()
        {
            ContentRequest Request = new ContentRequest("field/random/");
            Game.Instance.DefaultWebManager.Get(Request, LoadFieldTemplate);
        }
        private void LoadFieldTemplate(object obj)
        {
            GridProps NewGridProps = JsonUtility.FromJson<GridProps>(obj.ToString());
            FieldGenerated(NewGridProps);
        }
    }
}