using System;
using Csla;

namespace Templates
{
  [Serializable]
  public class EditableRootParent : BusinessBase<EditableRootParent>
  {
    #region Business Methods

    // TODO: add your own fields, properties and methods

    // example with private backing field
    private static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(p => p.Id);
    private int _Id = IdProperty.DefaultValue;
    public int Id
    {
      get { return GetProperty(IdProperty, _Id); }
      set { SetProperty(IdProperty, ref _Id, value); }
    }

    // example with managed backing field
    private static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
    public string Name
    {
      get { return GetProperty(NameProperty); }
      set { SetProperty(NameProperty, value); }
    }

    // example with Editable Child list 
    private static readonly PropertyInfo<EditableChildList> ChildListProperty = RegisterProperty<EditableChildList>(p => p.ChildList, "Child list");
    public EditableChildList ChildList
    {
      get { return GetProperty<EditableChildList>(ChildListProperty); }
    }

    // Example with Editable Child 
    private static readonly PropertyInfo<EditableChild> ChildProperty = RegisterProperty<EditableChild>(p => p.Child, "Child");
    public EditableChild Child
    {
      get { return GetProperty<EditableChild>(ChildProperty); }
    }
    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      // TODO: add validation rules
      //BusinessRules.AddRule(new Rule(), IdProperty);
    }

    private static void AddObjectAuthorizationRules()
    {
      // TODO: add authorization rules
      //BusinessRules.AddRule(...);
    }

    #endregion

    #region Factory Methods

    public static EditableRootParent NewEditableRootParent()
    {
      return DataPortal.Create<EditableRootParent>();
    }

    public static EditableRootParent GetEditableRootParent(int id)
    {
      return DataPortal.Fetch<EditableRootParent>(id);
    }

    public static void DeleteEditableRootParent(int id)
    {
      DataPortal.Delete<EditableRootParent>(id);
    }

    private EditableRootParent()
    { /* Require use of factory methods */ }

    #endregion

    #region Data Access

    [RunLocal]
    protected override void DataPortal_Create()
    {
      // TODO: load default values
      // omit this override if you have no defaults to set
      LoadProperty(ChildListProperty, EditableChildList.NewEditableChildList());
      LoadProperty(ChildProperty, EditableChild.NewEditableChild());
      base.DataPortal_Create();
    }

    private void DataPortal_Fetch(int criteria)
    {
      // TODO: load values
      LoadProperty(ChildListProperty, EditableChildList.GetEditableChildList(null));
      LoadProperty(ChildProperty, EditableChild.GetEditableChild(null));
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      // TODO: insert values
      FieldManager.UpdateChildren(this);
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Update()
    {
      // TODO: update values
      FieldManager.UpdateChildren(this);
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_DeleteSelf()
    {
      DataPortal_Delete(this.Id);
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    private void DataPortal_Delete(int criteria)
    {
      // TODO: delete values
      FieldManager.UpdateChildren(this);
    }

    #endregion
  }
}
