using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2Micro.BCLabManager.Shell.Model;

namespace O2Micro.BCLabManager.Shell.DataAccess
{    
    /// <summary>
    /// Event arguments used by RepositoryBase's IItemAdded event.
    /// </summary>
    public class ItemAddedEventArgs<T> : EventArgs
    {
        public ItemAddedEventArgs(T newItem)
        {
            this.NewItem = newItem;
        }

        public T NewItem { get; private set; }
    }
    public abstract class RepositoryBase<T>
    {
        #region Fields

        readonly List<T> _items;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Creates a new repository of items.
        /// </summary>
        /// <param name="customerDataFile">The relative path to an XML resource file that contains customer data.</param>
        /*public CustomerRepository(string customerDataFile)
        {
            _batterymodels = LoadCustomers(customerDataFile);
        }*/

        public RepositoryBase()
        {
            _items = new List<T>();
        }

        public RepositoryBase(List<T> items)
        {
            _items = items;
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Raised when a customer is placed into the repository.
        /// </summary>
        public event EventHandler<ItemAddedEventArgs<T>> ItemAdded;

        /// <summary>
        /// Places the specified customer into the repository.
        /// If the customer is already in the repository, an
        /// exception is not thrown.
        /// </summary>
        public void AddItem(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (!_items.Contains(item))
            {
                _items.Add(item);

                if (this.ItemAdded != null)
                    this.ItemAdded(this, new ItemAddedEventArgs<T>(item));
            }
        }

        /// <summary>
        /// Returns true if the specified customer exists in the
        /// repository, or false if it is not.
        /// </summary>
        public bool ContainsItem(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return _items.Contains(item);
        }

        /// <summary>
        /// Returns a shallow-copied list of all customers in the repository.
        /// </summary>
        public List<T> GetItems()
        {
            return new List<T>(_items);
        }

        public abstract void SerializeToDatabase();
        public abstract void DeserializeFromDatabase();
        #endregion // Public Interface

        #region Private Helpers

        /*static List<T> LoadCustomers(string customerDataFile)
        {
            // In a real application, the data would come from an external source,
            // but for this demo let's keep things simple and use a resource file.
            using (Stream stream = GetResourceStream(customerDataFile))
            using (XmlReader xmlRdr = new XmlTextReader(stream))
                return
                    (from customerElem in XDocument.Load(xmlRdr).Element("customers").Elements("customer")
                     select Customer.CreateCustomer(
                        (double)customerElem.Attribute("totalSales"),
                        (string)customerElem.Attribute("firstName"),
                        (string)customerElem.Attribute("lastName"),
                        (bool)customerElem.Attribute("isCompany"),
                        (string)customerElem.Attribute("email")
                         )).ToList();
        }*/

        /*static Stream GetResourceStream(string resourceFile)
        {
            Uri uri = new Uri(resourceFile, UriKind.RelativeOrAbsolute);

            StreamResourceInfo info = Application.GetResourceStream(uri);
            if (info == null || info.Stream == null)
                throw new ApplicationException("Missing resource file: " + resourceFile);

            return info.Stream;
        }*/

        #endregion // Private Helpers
    }

    public class BatteryTypeRepository : RepositoryBase<BatteryTypeClass>
    {
        public BatteryTypeRepository(List<BatteryTypeClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class BatteryRepository : RepositoryBase<BatteryClass>
    {
        public BatteryRepository(List<BatteryClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class ChamberRepository : RepositoryBase<ChamberClass>
    {
        public ChamberRepository(List<ChamberClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class TesterRepository : RepositoryBase<TesterClass>
    {
        public TesterRepository(List<TesterClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class ProgramRepository : RepositoryBase<ProgramClass>
    {
        public ProgramRepository(List<ProgramClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class SubProgramRepository : RepositoryBase<SubProgramClass>
    {
        public SubProgramRepository(List<SubProgramClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class RecipeRepository : RepositoryBase<RecipeClass>
    {
        public RecipeRepository(List<RecipeClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class TesterRecipeRepository : RepositoryBase<TesterRecipeClass>
    {
        public TesterRecipeRepository(List<TesterRecipeClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class ChamberRecipeRepository : RepositoryBase<ChamberRecipeClass>
    {
        public ChamberRecipeRepository(List<ChamberRecipeClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class RequestRepository : RepositoryBase<RequestClass>
    {
        public RequestRepository()
        {}
        public RequestRepository(List<RequestClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }

    public class ExecutorRepository : RepositoryBase<ExecutorClass>
    {
        public ExecutorRepository(List<ExecutorClass> items)
            : base(items)
        { 
        }
        public override void SerializeToDatabase()
        {
            throw new NotImplementedException();
        }
        public override void DeserializeFromDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
