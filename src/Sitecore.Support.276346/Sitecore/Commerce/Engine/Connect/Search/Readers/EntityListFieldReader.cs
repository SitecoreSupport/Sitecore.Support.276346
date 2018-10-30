namespace Sitecore.Commerce.Engine.Connect.Search.Readers
{
  using System.Collections.Generic;
  using System.Linq;
  using Abstractions;
  using ContentSearch;
  using ContentSearch.FieldReaders;
  using Diagnostics;
  using Sitecore.Data;
  using Sitecore.Data.Fields;

  /// <summary>
  ///     FieldReader for commerce fields of List type
  /// </summary>
  public class EntityListFieldReader : FieldReader
  {
    /// <summary>
    ///     Gets the fieldValue from the indexableField
    /// </summary>
    public override object GetFieldValue(IIndexableDataField indexableField)
    {
      var field = indexableField as SitecoreItemDataField;

      var resultList = new List<string>();

      var valueList = new List<string>();

      var fieldTypeManager = ContentSearchManager.Locator.GetInstance<BaseFieldTypeManager>();
      Assert.IsNotNull(fieldTypeManager, "fieldTypeManager != null");

      var singleTextField = fieldTypeManager.GetField(field) as TextField;
      if (singleTextField != null)
      {
        valueList = singleTextField.Value.Split('|').Distinct().ToList();

        foreach (var value in valueList)
        {
          var itm = value;

          if (ID.IsID(itm))
          {
            if (itm.Length == 38)
              itm = ShortID.Encode(itm).ToLowerInvariant();
            else
              itm = itm.Replace("-", "").ToLowerInvariant();

            resultList.Add(itm);
          }
        }
      }

      return resultList;
    }
  }
}