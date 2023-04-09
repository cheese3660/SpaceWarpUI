using UnityEngine;
using UnityEngine.UIElements;

public class ModListView: MonoBehaviour
{
    [SerializeField] private VisualTreeAsset ListEntryTemplate;

    private ModListController _modListController;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        _modListController = new ModListController();
        _modListController.InitializeLists(uiDocument.rootVisualElement, ListEntryTemplate);
    }
}