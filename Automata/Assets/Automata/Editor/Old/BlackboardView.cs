using UnityEditor.Experimental.GraphView;

namespace Automata.Editor
{
    /*
     * ARCHIVED CODE
     * Purpose: Blackboard Visual Editor
     * Archived By: James Greensill.
     */

    internal class BlackboardView : Blackboard
    {
        // private List<BlackboardValue> _ExposedValues;
        // private BlackboardContainer _BlackboardContainer;
        // private AutomataEditor _Editor;
        //
        // public BlackboardView(GraphView view) : base(view)
        // {
        //     _ExposedValues = new List<BlackboardValue>();
        // }
        //
        // public void Initialize(AutomataEditor editor)
        // {
        //     if (editor == null)
        //     {
        //         Debug.Log("Editor was null - BlackboardView.");
        //         return;
        //     }
        //
        //     RegisterCallback<ContextualMenuPopulateEvent>(_BuildContextualMenu);
        //
        //     addItemRequested += AddItemRequested;
        //
        //     _Editor = editor;
        // }
        //
        // private void AddItemRequested(Blackboard blackboard)
        // {
        //     AddToBlackboard(new BlackboardValue(""));
        //     AddToBlackboard(new BlackboardValue(0));
        // }
        //
        // public void AddToBlackboard(BlackboardValue value)
        // {
        //     string localName = value.PropertyName;
        //     var localValue = value.Read<object>();
        //
        //     var item = new BlackboardValue(localValue)
        //     {
        //         PropertyName = localName,
        //     };
        //
        //     var container = new VisualElement();
        //     var field = new BlackboardField(item);
        //     container.Add(field);
        //
        //     var propertyValueTextField = new TextField("Value:")
        //     {
        //         value = localValue.ToString()
        //     };
        //
        //     propertyValueTextField.RegisterValueChangedCallback(evt =>
        //     {
        //         var index = _ExposedValues.FindIndex(x => x.Guid == item.Guid);
        //         _ExposedValues[index].Write(evt.newValue);
        //     });
        //
        //     container.Add(new BlackboardRow(field, propertyValueTextField));
        //
        //     _ExposedValues.Add(item);
        //     AddToSelection(field);
        //     Add(container);
        // }
        //
        // public void PopulateBlackboard(BlackboardContainer blackboard)
        // {
        //     _BlackboardContainer = blackboard;
        //
        //     foreach (var value in _BlackboardContainer.Data)
        //     {
        //     }
        // }
        //
        // protected virtual void _BuildContextualMenu(ContextualMenuPopulateEvent evt)
        // {
        //     evt.menu.AppendAction("My Item", action => { }, DropdownMenuAction.AlwaysEnabled);
        // }
    }
}