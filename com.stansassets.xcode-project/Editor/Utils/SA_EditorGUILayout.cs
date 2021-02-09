using System.Collections.Generic;

namespace SA.Foundation.Editor
{
    public static class SA_EditorGUILayout
    {
        public static void ReorderablList<T>(IList<T> list, SA_ReorderablList.ItemName<T> itemName, SA_ReorderablList.ItemContent<T> itemContent = null, SA_ReorderablList.OnItemAdd onItemAdd = null, SA_ReorderablList.ItemContent<T> buttonsContent = null, SA_ReorderablList.ItemContent<T> itemStartUI = null)
        {
            SA_ReorderablList.Draw(list, itemName, itemContent, onItemAdd, buttonsContent, itemStartUI);
        }
    }
}
