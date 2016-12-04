using System;

namespace Common.Exceptions
{
    public class EditCollectionException : Exception
    {
        public string CollectionId { get; set; }
        public string MixId { get; set; }
        public bool isAdditionOperation { get; set; }

        public EditCollectionException() { }
        public EditCollectionException(string mixId, string collId, bool isAddn) : base(FormatMessage(mixId, collId, isAddn))
        {
            MixId = mixId;
            CollectionId = collId;
            isAdditionOperation = isAddn;
        }

        public static string FormatMessage(string mixId, string collId, bool isAddn)
        {
            string op = isAddn ? "inserted into" : "removed from";
            return string.Format("An error occurred while MixID: {0} was being {1} Collection Id: {2}", mixId, op, collId);
        }
    }
}
