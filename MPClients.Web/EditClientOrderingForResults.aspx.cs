using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MPClients.PageControllers;
using MPClients.DataAccess.Domain;
using MPClients.DataAccess.NHibernate;
using MPClients.Common;
using NHibernate;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MPClients
{
    public partial class EditClientOrderingForResults : BasePage
    {

        protected override void PageLoad()
        {
            if (!this.IsPostBack)
            {
                Session["GridItems"] = null;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            uxSave.Click += new EventHandler(uxSave_Click);
            uxAdd.Click += new EventHandler(uxAdd_Click);
            uxCategories.SelectedIndexChanged +=new RadComboBoxSelectedIndexChangedEventHandler(uxCategories_SelectedIndexChanged);
            base.OnInit(e);
        }

        protected void chkHasExpandedRange_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            bool status = chk.Checked;
            GridDataItem gdi = (GridDataItem)chk.NamingContainer;
            string clientId = gdi.GetDataKeyValue("ClientId").ToString();
            
            IList<ResultOrder> gridItems = GridItems;
            ResultOrder item = GetGridItem(gridItems, clientId);
            item.HasExpandedRange = status;
            GridItems = gridItems;
            BindDataGrid();

        }

        void uxSave_Click(object sender, EventArgs e)
        {
            IList<ResultOrder> gridItems = GridItems;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            SqlCommand cmdDeleteResultOrderCategory = new SqlCommand("spDeleteResultOrderByCategory", conn);
            cmdDeleteResultOrderCategory.Parameters.AddWithValue("@CategoryId", uxCategories.SelectedValue);
            cmdDeleteResultOrderCategory.CommandType = CommandType.StoredProcedure;

            SqlCommand cmdInsertResultOrder = new SqlCommand("spInsertResultOrder", conn);
            cmdInsertResultOrder.Parameters.AddWithValue("@CategoryId", uxCategories.SelectedValue);
            cmdInsertResultOrder.CommandType = CommandType.StoredProcedure;

            SqlParameter pClientId = new SqlParameter();
            SqlParameter pOrderPosition = new SqlParameter();
            SqlParameter pHasExpandedRange = new SqlParameter();
            pClientId.ParameterName = "@ClientId";
            pOrderPosition.ParameterName = "@OrderPosition";
            pHasExpandedRange.ParameterName = "@HasExpandedRange";

            conn.Open();
            SqlTransaction transaction = conn.BeginTransaction();
            cmdDeleteResultOrderCategory.Transaction = transaction;
            cmdInsertResultOrder.Transaction = transaction;

            try
            {
                // clear all for current category
                cmdDeleteResultOrderCategory.ExecuteNonQuery();

                // insert records
                foreach (ResultOrder item in gridItems)
                {
                    // can remove only if it's the first time
                    if (cmdInsertResultOrder.Parameters.Count > 1)
                    {
                        cmdInsertResultOrder.Parameters.Remove(pClientId);
                        cmdInsertResultOrder.Parameters.Remove(pOrderPosition);
                        cmdInsertResultOrder.Parameters.Remove(pHasExpandedRange);
                    }

                    pClientId.Value = item.ClientId;
                    pOrderPosition.Value = gridItems.IndexOf(item);
                    pHasExpandedRange.Value = item.HasExpandedRange;

                    cmdInsertResultOrder.Parameters.Add(pClientId);
                    cmdInsertResultOrder.Parameters.Add(pOrderPosition);
                    cmdInsertResultOrder.Parameters.Add(pHasExpandedRange);

                    cmdInsertResultOrder.ExecuteNonQuery();
                }
                transaction.Commit();
                MasterPage.DisplayMessage("Information updated.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MasterPage.DisplayMessage(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        void uxAdd_Click(object sender, EventArgs e)
        {
            if (uxClients.SelectedValue != null && uxCategories.SelectedValue != null)
            {
                const int intLengthOfClientId = 5;

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

                SqlCommand command = new SqlCommand("spGetClient", conn);
                command.Parameters.AddWithValue("@ClientId", uxClients.SelectedValue);
                command.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                IList<ResultOrder> gridItems = GridItems;
                ResultOrder item = new ResultOrder(uxClients.SelectedValue, 
                                    uxClients.SelectedValue + '-' + reader["Name"].ToString(),
                                    reader["Contact_person"].ToString(),
                                    // uxClients.Text.Substring(intLengthOfClientId+1,  uxClients.Text.Length - intLengthOfClientId-1), 
                                    uxCategories.SelectedValue, 
                                    uxCategories.Text, 
                                    gridItems.Count+1, 
                                    false);
                gridItems.Insert(gridItems.Count, item);
                GridItems = gridItems;
                BindDataGrid();

                conn.Close();
            }
            else
            {
                MasterPage.DisplayMessage("Missing category or client.");
            }
        }
        
        protected void uxGrid_ItemUpdated(object source, Telerik.Web.UI.GridUpdatedEventArgs e)
        {
            GridEditableItem gei = (GridEditableItem)e.Item;
            String clientId = gei.GetDataKeyValue("ClientId").ToString();

            IList<ResultOrder> gridItems = GridItems;
            ResultOrder item = GetGridItem(gridItems, clientId);

            //item.HasExpandedRange = status;
            GridItems = gridItems;
            BindDataGrid();

            if (e.Exception != null)
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
                NotifyUser("Client with ID " + clientId + " cannot be updated. Reason: " + e.Exception.Message);
            }
            else
            {
                NotifyUser("Client with ID " + clientId + " is updated!");
            }
        }

        protected void uxGrid_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            String clientId = dataItem.GetDataKeyValue("ClientId").ToString();

            IList<ResultOrder> gridItems = GridItems;
            ResultOrder item = GetGridItem(gridItems, clientId);
            gridItems.Remove(item);

            NotifyUser("Client with ID " + clientId + " is deleted!");
            GridItems = gridItems;
            BindDataGrid();
        }

        protected void uxGrid_EditCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            String clientId = dataItem.GetDataKeyValue("ClientId").ToString();
            BindDataGrid();
        }

        protected void uxGrid_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = (GridEditableItem)e.Item;
            String clientId = editableItem.GetDataKeyValue("ClientId").ToString();

            IList<ResultOrder> gridItems = GridItems;
            ResultOrder item = GetGridItem(gridItems, clientId);

            editableItem.UpdateValues(item);

            GridItems = gridItems;
            BindDataGrid();

            NotifyUser("Client with ID " + clientId + " is updated!");
        }

        protected void uxGrid_CancelCommand(object source, GridCommandEventArgs e)
        {
            BindDataGrid();
        }

        private void NotifyUser(string message)
        {
            RadListBoxItem commandListItem = new RadListBoxItem();
            commandListItem.Text = message;
            SavedChangesList.Items.Add(commandListItem);
        }

        protected void uxCategories_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["GridItems"] = null;
            BindDataGrid();
        }

        protected void uxClients_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            Helper.LoadClients(e, uxClients);
        }

        protected void uxCategories_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            Helper.LoadCategories(e, uxCategories);
        }

        void uxGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                BindDataGrid();
            }
            catch { }
        }

        private void BindDataGrid()
        {
            try
            {
                uxGrid.DataSource = GridItems;
                uxGrid.DataBind();

                //uxGrid.Visible = true;
                //_conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

                //SqlCommand command = new SqlCommand("spGetResultOrderByCategory", _conn);
                //command.Parameters.AddWithValue("@CategoryId", uxCategories.SelectedValue);
                //command.CommandType = CommandType.StoredProcedure;

                //_conn.Open();
                //_reader = command.ExecuteReader();

                //uxGrid.DataSource = _reader;
                //uxGrid.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void uxGrid_RowDrop(object sender, GridDragDropEventArgs e)
        {
            if (string.IsNullOrEmpty(e.HtmlElement))
            {

                if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == uxGrid.ClientID)
                {
                    //reorder items in grid
                    IList<ResultOrder> gridItems = GridItems;
                    ResultOrder item = GetGridItem(gridItems, (string)e.DestDataItem.GetDataKeyValue("ClientId"));
                    int destinationIndex = gridItems.IndexOf(item);

                    List<ResultOrder> itemsToMove = new List<ResultOrder>();
                    foreach (GridDataItem draggedItem in e.DraggedItems)
                    {
                        ResultOrder tmpItem = GetGridItem(gridItems, (string)draggedItem.GetDataKeyValue("ClientId"));
                        if (tmpItem != null)
                            itemsToMove.Add(tmpItem);
                    }

                    foreach (ResultOrder itemToMove in itemsToMove)
                    {
                        gridItems.Remove(itemToMove);
                        gridItems.Insert(destinationIndex, itemToMove);
                    }
                    GridItems = gridItems;
                    BindDataGrid();
                }
            }
        }

        protected IList<ResultOrder> GridItems
        {
            get
            {
                try
                {
                    object obj = Session["GridItems"];
                    if (obj == null)
                    {
                        obj = GetGridItems();
                        if (obj != null)
                        {
                            Session["GridItems"] = obj;
                        }
                        else
                        {
                            obj = new List<ResultOrder>();
                        }
                    }
                    return (IList<ResultOrder>)obj;
                }
                catch
                {
                    Session["GridItems"] = null;
                }
                return new List<ResultOrder>();
            }
            set { Session["GridItems"] = value; }
        }

        protected IList<ResultOrder> GetGridItems()
        {
            IList<ResultOrder> results = new List<ResultOrder>();
            if (uxCategories.SelectedValue != null)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

                SqlCommand command = new SqlCommand("spGetResultOrderByCategory", conn);
                command.Parameters.AddWithValue("@CategoryId", uxCategories.SelectedValue);
                command.CommandType = CommandType.StoredProcedure;

                conn.Open();
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string clientId = (!reader.IsDBNull(reader.GetOrdinal("Client_Id")))
                                                ? (string)reader.GetValue(reader.GetOrdinal("Client_Id"))
                                                : string.Empty;

                        string client = (!reader.IsDBNull(reader.GetOrdinal("Client")))
                                                ? (string)reader.GetValue(reader.GetOrdinal("Client"))
                                                : string.Empty;

                        string businessName = (!reader.IsDBNull(reader.GetOrdinal("BusinessName")))
                                                ? (string)reader.GetValue(reader.GetOrdinal("BusinessName"))
                                                : string.Empty;

                        string categoryId = (!reader.IsDBNull(reader.GetOrdinal("Category_Id")))
                                                ? (string)reader.GetValue(reader.GetOrdinal("Category_Id"))
                                                : string.Empty;

                        string categoryName = (!reader.IsDBNull(reader.GetOrdinal("CategoryIDandName")))
                                                ? (string)reader.GetValue(reader.GetOrdinal("CategoryIDandName"))
                                                : string.Empty;

                        int orderPosition = (!reader.IsDBNull(reader.GetOrdinal("OrderPosition")))
                                                ? Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderPosition")).ToString())
                                                : 0;

                        bool hasExpandedRange = (!reader.IsDBNull(reader.GetOrdinal("HasExpandedRange")))
                                                ? (bool)reader.GetValue(reader.GetOrdinal("HasExpandedRange"))
                                                : false;

                        results.Add(new ResultOrder(clientId, client, businessName, categoryId, categoryName, orderPosition, hasExpandedRange));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    results.Clear();
                }
                finally
                {
                    conn.Close();
                }
            }
            return results;
        }

        private static ResultOrder GetGridItem(IEnumerable<ResultOrder> itemsToSearchIn, string clientId)
        {
            foreach (ResultOrder item in itemsToSearchIn)
            {
                if (item.ClientId == clientId)
                {
                    return item;
                }
            }
            return null;
        }

        protected class ResultOrder
        {
            private string _clientId;
            private string _client;
            private string _businessName;
            private string _categoryId;
            private string _categoryName;
            private int _orderPosition;
            private bool _hasExpandedRange;

            public ResultOrder(string clientId,
                                string client,
                                string businessName,
                                string categoryId,
                                string categoryName,
                                int orderPosition,
                                bool hasExpandedRange
                 )
            {
                _clientId = clientId;
                _client = client;
                _businessName = businessName;
                _categoryId = categoryId;
                _categoryName = categoryName;
                _orderPosition = orderPosition;
                _hasExpandedRange = hasExpandedRange;
            }

            public string ClientId
            {
                get { return _clientId; }
            }

            public string BusinessName
            {
                get { return _businessName; }
            }

            public string Client
            {
                get { return _client; }
            }

            public string CategoryId
            {
                get { return _categoryId; }
            }

            public string CategoryName
            {
                get { return _categoryName; }
            }

            public int OrderPosition
            {
                get { return _orderPosition; }
            }

            public bool HasExpandedRange
            {
                get { return _hasExpandedRange; }
                set { _hasExpandedRange = value; }
            }
        }
    }
}