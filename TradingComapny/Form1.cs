using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradingComapny
{
    public partial class TradingCO : Form
    {

        warehouseDBEntities3 entitiy;
        int reportFlag;
        public TradingCO()
        {
            InitializeComponent();
            entitiy = new warehouseDBEntities3();
            reportFlag = 0;

        }

        //formload
        private void TradingCO_Load(object sender, EventArgs e)
        {
            fillWarehouseGrid();
            fillproductGrid();
            fillsupplierGrid();
            fillcustomerGrid();
            fillinboundGrid();
            filloutboundGrid();
            fillFromToStoresGridView();


            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
        public void fillWarehouseGrid()
        {
            WH_GV.Rows.Clear();
            foreach(var store in entitiy.WareHouses)
            {
                WH_GV.Rows.Add(store.WareHouse_name,store.WareHouse_address,store.WareHouse_keeper);
            }

        }

        private void WH_GV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            WH_prdocut_GV.Rows.Clear();
          string wH=  WH_GV.SelectedRows[0].Cells[0].Value.ToString();

           
            if( WH_GV.SelectedRows.Count==1)
            {
                var products =  entitiy.product_WareHouse.Where(p => p.WareHouse_name == wH).Select(p => new { p.product_ID, p.product.product_name }).Distinct();
                foreach (var p in products)
                {
                    WH_prdocut_GV.Rows.Add(p.product_ID, p.product_name, "");
                }

            }
            else 
            {
                List<string> storenames = new List<string>();

                for(int i=0;i< WH_GV.SelectedRows.Count;i++)
                {
                    storenames.Add(WH_GV.SelectedRows[i].Cells[0].Value.ToString());
                }
                var products = entitiy.product_WareHouse.Where(p => storenames.Contains(p.WareHouse_name))
                    .Select(p => new { p.product_ID, p.product.product_name ,p.WareHouse_name}).Distinct();

                foreach (var p in products)
                {
                    WH_prdocut_GV.Rows.Add(p.product_ID, p.product_name,p.WareHouse_name);
                }
               

            }
            Whouse_name.Text = wH;
            Whouse_Address.Text = WH_GV.SelectedRows[0].Cells[1].Value.ToString();
            WH_manger.Text = WH_GV.SelectedRows[0].Cells[2].Value.ToString();

        }

        private void Insert_WH_Click(object sender, EventArgs e)
        {

            var stores = entitiy.WareHouses.Find(Whouse_name.Text);
            if (stores==null)
            {

            WareHouse ware = new WareHouse();
            ware.WareHouse_name = Whouse_name.Text;
            ware.WareHouse_address=Whouse_Address.Text;
           ware.WareHouse_keeper= WH_manger.Text;
            entitiy.WareHouses.Add(ware);
            entitiy.SaveChanges();
            fillWarehouseGrid();
            }
            else
            {
                MessageBox.Show("warehouse name already exists");
            }
        }

        private void Update_WH_Click(object sender, EventArgs e)
        {
            var stores = entitiy.WareHouses.Find(Whouse_name.Text);
            if (stores != null)
            {

                
                stores.WareHouse_name = Whouse_name.Text;
                stores.WareHouse_address = Whouse_Address.Text;
                stores.WareHouse_keeper = WH_manger.Text;
                
                entitiy.SaveChanges();
                fillWarehouseGrid();
                MessageBox.Show("warehouse updated");
            }
            else
            {
                MessageBox.Show("warehouse name already exists");
            }
        }

        // product tab
         public void fillproductGrid()
        {
            product_GV.Rows.Clear();
            foreach(var product in entitiy.products)
            {
                product_GV.Rows.Add(product.product_ID,product.product_name,product.expire_period);
            }

        }

        private void product_GV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string id_product = product_GV.SelectedRows[0].Cells[0].Value.ToString();
            string name_product = product_GV.SelectedRows[0].Cells[1].Value.ToString();
            string date_product = product_GV.SelectedRows[0].Cells[2].Value.ToString();

            Product_ID.Text = id_product;
            Product_name.Text = name_product;
            ex_period.Text = date_product;
            // get Warehouse names 
            var Whouse = entitiy.product_WareHouse.Where(s => s.product_ID == int.Parse(id_product))
                    .Select(s => s.WareHouse_name).Distinct();

            //clear all fields
            textBox1.Text= string.Empty;
            WH_P.Items.Clear();
            pro_date_p.Items.Clear();
            foreach (var wh in Whouse)
            {
                WH_P.Items.Add(wh);
            }
            //select the first wh
            if (WH_P.Items.Count > 0)
            {
                WH_P.SelectedIndex = 0;
            }

        }

        private void insert_product_Click(object sender, EventArgs e)
        {
            try
            {
                int id_product = int.Parse(Product_ID.Text);
                var product = entitiy.products.Find(id_product);
                if (product == null)
                {

                    product newpro = new product();
                    newpro.product_ID = id_product;
                    newpro.product_name = Product_name.Text;
                    newpro.expire_period = int.Parse(ex_period.Text);
                    entitiy.products.Add(newpro);
                    entitiy.SaveChanges();
                    fillproductGrid();
                }
                else
                {
                    MessageBox.Show("ID already exists");
                }
            }
            catch
            {
                MessageBox.Show("Check full info and noo product is inserted");
            }

        }

        private void update_product_Click(object sender, EventArgs e)
        {
            try
            {

            int id_product = int.Parse(Product_ID.Text);
            var product = entitiy.products.Find(id_product);
            if (product != null)
            {

                
                product.product_name = Product_name.Text;
                product.expire_period = int.Parse(ex_period.Text);
                
                entitiy.SaveChanges();
                fillproductGrid();
            }
            else
            {
                MessageBox.Show("ID is already existing");
            }
            }
            catch
            {
                MessageBox.Show("Check full info product did not get updated");
            }
        }
        private void WH_P_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get the production dates available in the specified store of that product
            string Wh_Name = WH_P.SelectedItem.ToString();
            int productID = int.Parse(product_GV.SelectedRows[0].Cells[0].Value.ToString());
            var prodDates = entitiy.product_WareHouse.Where(p => p.WareHouse_name == Wh_Name && p.product_ID == productID)
                .Select(p => p.production_date);

            pro_date_p.Items.Clear();
            textBox1.Text = "";

            foreach (var prodDate in prodDates)
            {
                pro_date_p.Items.Add(prodDate.ToString().Split(' ')[0]);
            }
            //select the first production date
            if (pro_date_p.Items.Count > 0)
            {
                pro_date_p.SelectedIndex = 0;
            }
        }

        private void pro_date_p_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != string.Empty)
                {
                    string wh_name = WH_P.SelectedItem.ToString();
                    int productID = int.Parse(product_GV.SelectedRows[0].Cells[0].Value.ToString());
                    DateTime prodDate = DateTime.Parse(pro_date_p.SelectedItem.ToString());
                    int quantity = entitiy.product_WareHouse.Where(p => p.WareHouse_name == wh_name && p.product_ID == productID && p.production_date == prodDate)
                        .Select(p => p.quantity).First();
                    textBox1.Text = quantity.ToString();
                }
                else
                {
                    textBox1.Text = string.Empty;
                    MessageBox.Show("Error has happened please enter valid date and quantity");
                }
            }
            catch
            {
                textBox1.Text = string.Empty;
                MessageBox.Show("Error has happened please enter valid date and quantity");


            }

        }
        // supllier tab
        public void fillsupplierGrid()
        {
            Supplier_GV.Rows.Clear();
            foreach (var supplier in entitiy.suppliers)
            {
                Supplier_GV.Rows.Add(supplier.supplier_ID, supplier.supplier_name, supplier.phone, supplier.fax, supplier.mobile, supplier.email, supplier.website);
            }

        }

        private void Supplier_GV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string id_supplier = Supplier_GV.SelectedRows[0].Cells[0].Value.ToString();
            string name_supplier = Supplier_GV.SelectedRows[0].Cells[1].Value.ToString();
            string phone_supplier = Supplier_GV.SelectedRows[0].Cells[2].Value.ToString();
            string fax_supplier = Supplier_GV.SelectedRows[0].Cells[3].Value.ToString();
            string mobile_supplier = Supplier_GV.SelectedRows[0].Cells[4].Value.ToString();
            string email_supplier = Supplier_GV.SelectedRows[0].Cells[5].Value.ToString();
            string site_supplier = Supplier_GV.SelectedRows[0].Cells[6].Value.ToString();


            S_ID.Text = id_supplier;
            S_name.Text = name_supplier;
            S_Phone.Text = phone_supplier;
            S_Fax.Text = fax_supplier;
            S_Mobile.Text = mobile_supplier;
            
            S_email.Text = email_supplier;
            S_web.Text = site_supplier;

        }

       

        

        private void Supplier_insert_Click(object sender, EventArgs e)
        {
            int id_supplier = int.Parse(S_ID.Text);
            var supplier = entitiy.suppliers.Find(id_supplier);
            if (supplier == null)
            {

                supplier newpro = new supplier();
                newpro.supplier_ID = int.Parse(S_ID.Text);
                newpro.supplier_name = S_name.Text;
                newpro.phone = S_Phone.Text;
                newpro.email = S_email.Text;
                newpro.fax = S_Fax.Text;
                newpro.mobile = S_Mobile.Text;
                newpro.website = S_web.Text;
                entitiy.suppliers.Add(newpro);
                entitiy.SaveChanges();
                fillsupplierGrid();
            }
            else
            {
                MessageBox.Show("ID already exists");
            }
        }

        private void Supplier_update_Click(object sender, EventArgs e)
        {
            int id_supplier = int.Parse(S_ID.Text);
            var supplier = entitiy.suppliers.Find(id_supplier);
            if (supplier != null)
            {


                supplier.supplier_name = S_name.Text; ;
                supplier.phone = S_Phone.Text;
                supplier.fax = S_Fax.Text;
                supplier.mobile = S_Mobile.Text;
                supplier.email = S_email.Text;
                supplier.website = S_web.Text;

                entitiy.SaveChanges();
                fillsupplierGrid();
            }
            else
            {
                MessageBox.Show("ID is already existing");
            }
        }
            // customer tab
            public void fillcustomerGrid()
            {
                Customer_GV.Rows.Clear();
                foreach (var customer in entitiy.clients)
                {
                    Customer_GV.Rows.Add(customer.client_ID, customer.client_name, customer.phone, customer.fax, customer.mobile, customer.email, customer.website);
                }

            }

            private void Customer_GV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
            {
                string id_supplier = Customer_GV.SelectedRows[0].Cells[0].Value.ToString();
                string name_supplier = Customer_GV.SelectedRows[0].Cells[1].Value.ToString();
                string phone_supplier = Customer_GV.SelectedRows[0].Cells[2].Value.ToString();
                string fax_supplier = Customer_GV.SelectedRows[0].Cells[3].Value.ToString();
                string mobile_supplier = Customer_GV.SelectedRows[0].Cells[4].Value.ToString();
                string email_supplier = Customer_GV.SelectedRows[0].Cells[5].Value.ToString();
                string site_supplier = Customer_GV.SelectedRows[0].Cells[6].Value.ToString();


                C_ID.Text = id_supplier;
                C_name.Text = name_supplier;
                C_phone.Text = phone_supplier;
                C_fax.Text = fax_supplier;
                C_Mobile.Text = mobile_supplier;
                
                C_email.Text = email_supplier;
                C_site.Text = site_supplier;

            }


            

        private void Customer_insert_Click_1(object sender, EventArgs e)
        {
            int id_customer = int.Parse(C_ID.Text);
            var customer = entitiy.clients.Find(id_customer);
            if (customer == null)
            {

                client newpro = new client();
                newpro.client_ID = int.Parse(C_ID.Text);
                newpro.client_name = C_name.Text;
                newpro.phone = C_phone.Text;
                newpro.email = C_email.Text;
                newpro.fax = C_fax.Text;
                newpro.mobile = C_Mobile.Text;
                newpro.website = C_site.Text;
                entitiy.clients.Add(newpro);
                entitiy.SaveChanges();
                fillcustomerGrid();
            }
            else
            {
                MessageBox.Show("ID already exists");
            }
        }

        private void Customer_update_Click_1(object sender, EventArgs e)
        {
            int id_customer = int.Parse(C_ID.Text);
            var customer = entitiy.clients.Find(id_customer);
            if (customer != null)
            {


                customer.client_name = C_name.Text; ;
                customer.phone = C_phone.Text;
                customer.fax = C_fax.Text;
                customer.mobile = C_Mobile.Text;
                customer.email = C_email.Text;
                customer.website = C_site.Text;

                entitiy.SaveChanges();
                fillcustomerGrid();
            }
            else
            {
                MessageBox.Show("ID is already existing");
            }
        }

        // inbound tab
        public void fillinboundGrid()
        {
            inbond_supply_GV.Rows.Clear();
            foreach (var recite in entitiy.supplierRequest_details)
            {
                var suplierName = entitiy.suppliers.Where(s => s.supplier_ID == recite.supplier_requests.supplier_ID).Select(s => s.supplier_name).First().ToString();
                
                inbond_supply_GV.Rows.Add(recite.supplierRequest_ID, suplierName, recite.store_name,recite.product_ID,recite.input_quantity);
            }
            inWH_name.Items.Clear();
            foreach (var store in entitiy.WareHouses)
            {
                inWH_name.Items.Add(store.WareHouse_name);
            }

        }
        private void inbond_supply_GV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int reqID = int.Parse(inbond_supply_GV.SelectedRows[0].Cells[0].Value.ToString());
            var request = entitiy.supplierRequest_details.Where(r => r.supplierRequest_ID == reqID)
                .Select(r => r).FirstOrDefault();
            var inreqProducts = entitiy.supplierRequest_details.Where(r => r.supplierRequest_ID == reqID)
                .Select(r => r);
            //fill the products in that request
            inboundPRO_GV.Rows.Clear();
            foreach (var prod in inreqProducts)
            {
                inboundPRO_GV.Rows.Add(prod.product_ID, prod.product.product_name, prod.input_quantity,prod.supplier_requests.date);
            }
            InRe_ID.Text = request.supplierRequest_ID.ToString();
            inSupplier_name.Text = request.supplier_requests.supplier.supplier_name;
            inWH_name.Text = request.store_name;// warehouse name
           textBox2.Text = request.supplier_requests.date.ToString().Split(' ')[0];

        }
        private void inbound_insert_Click(object sender, EventArgs e)
        {
            
            //get the request and product details
            int reqID = int.Parse(InRe_ID.Text);
            string WHName = inWH_name.SelectedItem.ToString();
            DateTime date = DateTime.Parse(textBox2.Text);

            int prodID = int.Parse(inbound_product_ID.Text);
            DateTime prodDate = DateTime.Parse(inPro_date.Text);
            int quantity = int.Parse(inQuantity.Text);

            //check if the supplier exist in the warehouse system
            var supID = entitiy.suppliers.Where(s => s.supplier_name == inSupplier_name.Text)
                .Select(s => s.supplier_ID).FirstOrDefault();
            if (supID != 0)
            {
                //check if the product exist in the warehouse system
                var product = entitiy.products.Where(p => p.product_ID == prodID)
                    .Select(p => p).FirstOrDefault();
                if (product != null)
                {
                    //check if the supplier request id is new
                    var supplyrequest = entitiy.supplier_requests.Where(sr => sr.inRequest_ID == reqID)
                        .Select(sr => sr).FirstOrDefault();
                    if (supplyrequest == null)
                    {
                        //insert in supplier requests table
                        supplier_requests inRequest = new supplier_requests();
                        inRequest.inRequest_ID = reqID;
                        inRequest.supplier_ID = supID;
                        inRequest.date = date;
                        entitiy.supplier_requests.Add(inRequest);
                        entitiy.SaveChanges();
                    }
                    
                    //insert in supplier-requests details table
                    supplierRequest_details inReciteDetails = new supplierRequest_details();

                    inReciteDetails.supplierRequest_ID = reqID;
                    inReciteDetails.product_ID = prodID;
                    inReciteDetails.store_name = WHName;
                    inReciteDetails.input_quantity = quantity;
                    inReciteDetails.Production_date = prodDate;
                    entitiy.supplierRequest_details.Add(inReciteDetails);

                    //update the product stores table with the incoming quantity
                    var productStore = entitiy.product_WareHouse.Where(ps => ps.product_ID == prodID && ps.WareHouse_name == WHName && ps.production_date == prodDate)
                        .Select(ps => ps).FirstOrDefault();
                    //check if the product exists in that store
                    if (productStore == null)
                    {
                        product_WareHouse ps = new product_WareHouse();
                        ps.product_ID = prodID;
                        ps.WareHouse_name = WHName;
                        ps.production_date = prodDate;
                        ps.quantity = 0;
                        entitiy.product_WareHouse.Add(ps);
                        productStore = ps;
                    }
                    productStore.quantity += quantity;
                    entitiy.SaveChanges();
                    //form load
                    TradingCO_Load(null,null);
                }
                else
                {
                    MessageBox.Show("this product doesn't exist");
                    
                }
            }
            else
            {
                MessageBox.Show("this supplier doesn't exist");
            }

        }

        private void inbound_update_Click(object sender, EventArgs e)
        {
            
            
                if (InRe_ID.Text != null && inWH_name.SelectedItem.ToString() != null && inbound_product_ID.Text != null && inPro_date.Text != null && inQuantity.Text != null)
                {
                    int reqID = int.Parse(InRe_ID.Text);
                    string storeName = inWH_name.SelectedItem.ToString();
                    DateTime date = DateTime.Parse(textBox2.Text);

                    int prodID = int.Parse(inbound_product_ID.Text);
                    DateTime prodDate = DateTime.Parse(inPro_date.Text);
                    int quantity = int.Parse(inQuantity.Text);

                    //get the old quantity
                    var oldQuantity = entitiy.supplierRequest_details
                        .Where(sr => sr.supplierRequest_ID == reqID && sr.product_ID == prodID && sr.store_name == storeName && sr.Production_date == prodDate)
                            .Select(ps => ps.input_quantity).FirstOrDefault();
                    // supplier id from his name
                    var supID = entitiy.suppliers.Where(s => s.supplier_name == inSupplier_name.Text)
                    .Select(s => s.supplier_ID).FirstOrDefault();
                // supplyrecite
                var supplyrequest = entitiy.supplier_requests.Where(sr => sr.inRequest_ID == reqID)
                            .Select(sr => sr).FirstOrDefault();
                    //supply recite details to edit the quantity
                    var supplyrequestDetails = entitiy.supplierRequest_details
                        .Where(sr => sr.supplierRequest_ID == reqID && sr.product_ID == prodID && sr.store_name == storeName && sr.Production_date == prodDate)
                            .Select(ps => ps).FirstOrDefault();

                    if (supID != 0)
                    {
                        
                        supplyrequest.supplier_ID = supID;
                        supplyrequest.date = date;
                       
                        supplyrequestDetails.input_quantity = quantity;

                        
                        var productStore = entitiy.product_WareHouse.Where(ps => ps.product_ID == prodID && ps.WareHouse_name == storeName && ps.production_date == prodDate)
                            .Select(ps => ps).FirstOrDefault();
                        //check if the product exists in that store
                        if (productStore != null)
                        {
                            productStore.quantity = productStore.quantity + quantity - oldQuantity;
                            entitiy.SaveChanges();
                        }
                    TradingCO_Load(null, null);
                }
                    else
                    {
                        MessageBox.Show("this supplier doesn't exist add supplier first");
                    }
                }
            
        }
        private void inboundPRO_GV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int reqID = int.Parse(inbond_supply_GV.SelectedRows[0].Cells[0].Value.ToString());
            int prodID = int.Parse(inboundPRO_GV.SelectedRows[0].Cells[0].Value.ToString());
            DateTime proDate = DateTime.Parse(inbond_supply_GV.SelectedRows[0].Cells[3].Value.ToString());
            var inProduct = entitiy.supplierRequest_details.Where(r => r.supplierRequest_ID == reqID && r.product_ID == prodID && r.Production_date == proDate).Select(r => r).FirstOrDefault();

           inbound_product_ID.Text = prodID.ToString();
            inPro_date.Text = inProduct.Production_date.ToString().Split(' ')[0];
            inPro_exs.Text = inProduct.product.expire_period.ToString();
            inQuantity.Text = inProduct.input_quantity.ToString();
        }


        // outbound tab---------------------------
        public void filloutboundGrid()
        {
            outbound_Customer_GV.Rows.Clear();
            foreach (var recite in entitiy.client_requests)
            {
                outbound_Customer_GV.Rows.Add(recite.outRequest_ID, recite.client.client_name,recite.date);
            }
            out_wh_name.Items.Clear();
            foreach (var wh in entitiy.WareHouses)
            {
                out_wh_name.Items.Add(wh.WareHouse_name);
            }

        }
        private void outbound_Customer_GV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int reqID = int.Parse(outbound_Customer_GV.SelectedRows[0].Cells[0].Value.ToString());
            var request = entitiy.clientRequest_details.Where(r => r.clientRequets_ID == reqID)
                .Select(r => r).FirstOrDefault();
            var outReqProducts = entitiy.clientRequest_details.Where(r => r.clientRequets_ID == reqID)
                .Select(r => r);
            //fill the products in that recites
            out_product_GV.Rows.Clear();
            foreach (var prod in outReqProducts)
            {
                out_product_GV.Rows.Add(prod.product_ID, prod.product.product_name, prod.output_quantity, prod.Production_date);
            }
            out_recite_id.Text = request.clientRequets_ID.ToString();
            out_cus_name.Text = request.client_requests.client.client_name;
            out_wh_name.Text = request.store_name;
            out_recite_date.Text = request.client_requests.date.ToString().Split(' ')[0];

        }

        private void Insert_outbuond_Click(object sender, EventArgs e)
        {

            
            int reqID = int.Parse(out_recite_id.Text);
            string storeName = out_wh_name.SelectedItem.ToString();
            DateTime date = DateTime.Parse(out_recite_date.Text);

            int prodID = int.Parse(out_prodcut_id.Text);
            DateTime prodDate = DateTime.Parse(out_prod_date.Text);
            int quantity = int.Parse(out_Quantity.Text);

           
            var clientID = entitiy.clients.Where(c => c.client_name == out_cus_name.Text)
                .Select(c => c.client_ID).FirstOrDefault();

            if (clientID != 0)
            {
                //check if the product exist in the warehouse system
                var product = entitiy.products.Where(p => p.product_ID == prodID)
                    .Select(p => p).FirstOrDefault();
                if (product != null)
                {
                    //check if the product exists
                    
                    var productStore = entitiy.product_WareHouse.Where(ps => ps.product_ID == prodID && ps.WareHouse_name == storeName && ps.production_date == prodDate)
                       .Select(ps => ps).FirstOrDefault();
                    if (productStore != null)
                    {
                        //check if the available quantity is more than the required quantity
                        if (productStore.quantity >= quantity)
                        {
                            //check if the client request id is new
                            var purchaseRequest = entitiy.client_requests.Where(cr => cr.outRequest_ID == reqID)
                                .Select(sr => sr).FirstOrDefault();
                            if (purchaseRequest == null)
                            {
                                //insert in client requests table
                                client_requests outRequest = new client_requests();
                                outRequest.outRequest_ID = reqID;
                                outRequest.client_ID = clientID;
                                outRequest.date = date;
                                entitiy.client_requests.Add(outRequest);
                                entitiy.SaveChanges();
                            }
                            

                            //insert in clients-requests  table
                            clientRequest_details outRequestDetails = new clientRequest_details();
                            outRequestDetails.clientRequets_ID = reqID;
                            outRequestDetails.product_ID = prodID;
                            outRequestDetails.store_name = storeName;
                            outRequestDetails.output_quantity = quantity;
                            outRequestDetails.Production_date = prodDate;
                            entitiy.clientRequest_details.Add(outRequestDetails);

                            //update the product stores table with the incoming quantity
                            productStore.quantity -= quantity;
                            entitiy.SaveChanges();
                            TradingCO_Load(null, null);
                        }
                        else
                        {
                            MessageBox.Show($"the  quantity isn't available!\n the available  {productStore.quantity} left in store");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"the  product isn't available!\n in {productStore.WareHouse_name} store");
                    }
                }
                else
                {
                    MessageBox.Show("this product doesn't exist");
                    
                }
            }
            else
            {
                MessageBox.Show("this Customer doesn't exist");
            }
        }

        private void update_outbuond_Click(object sender, EventArgs e)
        {
            if (out_recite_id.Text != null && out_wh_name.SelectedItem.ToString() != null && out_prodcut_id.Text != null && out_Quantity.Text != null && out_prod_date.Text != null)
            {
                int reqID = int.Parse(out_recite_id.Text);
                string storeName = out_wh_name.SelectedItem.ToString();
                DateTime date = DateTime.Parse(out_recite_date.Text);

                int prodID = int.Parse(out_prodcut_id.Text);
                DateTime prodDate = DateTime.Parse(out_prod_date.Text);
                int quantity = int.Parse(out_Quantity.Text);

                //get the old quantity
                var oldQuantity = entitiy.clientRequest_details
                    .Where(cr => cr.clientRequets_ID == reqID && cr.product_ID == prodID && cr.store_name == storeName && cr.Production_date == prodDate)
                        .Select(ps => ps.output_quantity).FirstOrDefault();
                //get the client id from his name
                var clientID = entitiy.clients.Where(c => c.client_name == out_cus_name.Text)
                .Select(c => c.client_ID).FirstOrDefault();
                //get the client request
                var clientRequest = entitiy.client_requests.Where(cr => cr.outRequest_ID == reqID)
                        .Select(cr => cr).FirstOrDefault();
                // supply recite  details to edit the quantity
                var clientRequestDetails = entitiy.clientRequest_details
                    .Where(cr => cr.clientRequets_ID == reqID && cr.product_ID == prodID && cr.store_name == storeName && cr.Production_date == prodDate)
                        .Select(ps => ps).FirstOrDefault();

                if (clientID != 0)
                {
                    //update the client recite table
                    clientRequest.client_ID = clientID;
                    clientRequest.date = date;
                    //update the client recite details table
                    clientRequestDetails.output_quantity = quantity;

                    //update the product whouse table with the updated quantity
                    var productStore = entitiy.product_WareHouse.Where(ps => ps.product_ID == prodID && ps.WareHouse_name == storeName && ps.production_date == prodDate)
                        .Select(ps => ps).FirstOrDefault();
                    //check if the product exists in that whouse
                    if (productStore != null)
                    {
                        productStore.quantity = productStore.quantity - quantity + oldQuantity;
                        entitiy.SaveChanges();
                    }
                    TradingCO_Load(null, null);
                }
                else
                {
                    MessageBox.Show("this Customer doesn't exist");
                }

            }
        }

        // move product tab-----------------
        public void fillFromToStoresGridView()
        {
            //from store
            dataGridView6.Rows.Clear();
            foreach (var store in entitiy.WareHouses)
            {
                dataGridView6.Rows.Add(store.WareHouse_name);
            }
            // to store
            dataGridView5.Rows.Clear();
            foreach (var store in entitiy.WareHouses)
            {
                dataGridView5.Rows.Add(store.WareHouse_name);
            }
        }

        private void dataGridView6_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string storeName = dataGridView6.SelectedRows[0].Cells[0].Value.ToString();
            var products = entitiy.product_WareHouse.Where(p => p.WareHouse_name == storeName)
                .Select(p => new { p.product_ID, p.product.product_name }).Distinct();

            //select first product element
            if (Product_ID_From.Items.Count > 0)
            {
                Product_ID_From.SelectedIndex = 0;
                Product_Name_From.SelectedIndex = 0;
            }
            //fill products Id and name
            Product_ID_From.Items.Clear();
            Product_ID_From.Text = "";
            foreach (var product in products)
            {
                Product_ID_From.Items.Add(product.product_ID);
            }
            Product_Name_From.Items.Clear();
            Product_Name_From.Text = "";
            foreach (var product in products)
            {
                Product_Name_From.Items.Add(product.product_name);
            }
            //clear the form
            Qantity_p.Text = "";
            P_date.Text = "";
            R_quantity.Text = "";
            Expire_date.Text = "";
            
        }

        private void Product_Name_From_SelectedIndexChanged(object sender, EventArgs e)
        {
            Product_ID_From.SelectedIndex = Product_Name_From.SelectedIndex;
        }

        private void Product_ID_From_SelectedIndexChanged(object sender, EventArgs e)
        {
            Product_Name_From.SelectedIndex = Product_ID_From.SelectedIndex;
            int prodID = int.Parse(Product_ID_From.SelectedItem.ToString());
            string storeName = dataGridView6.SelectedRows[0].Cells[0].Value.ToString();
            var prodStores = entitiy.product_WareHouse.Where(p => p.product_ID == prodID && p.WareHouse_name == storeName)
                .Select(p => p);

            P_date.Items.Clear();
            foreach (var product in prodStores)
            {
                P_date.Items.Add(product.production_date.ToString().Split(' ')[0]);
            }
            Expire_date.Text = entitiy.products.Where(p => p.product_ID == prodID).Select(p => p.expire_period).First().ToString();


        }

        private void P_date_SelectedIndexChanged(object sender, EventArgs e)
        {
            int prodID = int.Parse(Product_ID_From.SelectedItem.ToString());
            string storeName = dataGridView6.SelectedRows[0].Cells[0].Value.ToString();
            DateTime prodDate = DateTime.Parse(P_date.SelectedItem.ToString());
            int quantityInWare = entitiy.product_WareHouse.Where(p => p.product_ID == prodID && p.WareHouse_name == storeName && p.production_date == prodDate)
                .Select(p => p.quantity).First();
            Qantity_p.Text = quantityInWare.ToString();

        }

        private void Transfer_btn_Click(object sender, EventArgs e)
        {
            // required quantity confirm and check
            int Flag;
           
            //try to parse the quantity
            int.TryParse(R_quantity.Text, out Flag);
            if (R_quantity.Text != null && Flag != 0 && R_quantity.Text !="")
            {
                int RQuantity = int.Parse(R_quantity.Text);
                int WH_Quantity = int.Parse(Qantity_p.Text);
                string fromstoreName = dataGridView6.SelectedRows[0].Cells[0].Value.ToString();
                string toStoreName = dataGridView5.SelectedRows[0].Cells[0].Value.ToString();

                int prodID = int.Parse(Product_ID_From.SelectedItem.ToString());
                // parse the string to date ------
                DateTime PDate = DateTime.Parse(P_date.SelectedItem.ToString());


                if (RQuantity <= WH_Quantity)
                {
                    //subtract the Transfer quantity from Whouse from
                    var prodWhouseFrom = entitiy.product_WareHouse.Where(p => p.product_ID == prodID && p.WareHouse_name == fromstoreName && p.production_date == PDate)
                        .Select(p => p).First();
                    prodWhouseFrom.quantity -= RQuantity;
                    //add the transfered quantity to Whouse 2 if the product exists or create new  entry for product with the Transfered quantity
                    var prodWhouseTo = entitiy.product_WareHouse.Where(p => p.product_ID == prodID && p.WareHouse_name == toStoreName && p.production_date == PDate)
                        .Select(p => p).FirstOrDefault();
                    //Trnasfer the product
                    if (prodWhouseTo != null) //the product present in the to warehouse
                    {
                        prodWhouseTo.quantity += RQuantity;
                    }
                    else        //the product doesnot exist in the from warehouse
                    {
                        prodWhouseTo = new product_WareHouse();
                        prodWhouseTo.product_ID = prodID;
                        prodWhouseTo.WareHouse_name = toStoreName;
                        prodWhouseTo.production_date = PDate;
                        prodWhouseTo.quantity = RQuantity;
                        entitiy.product_WareHouse.Add(prodWhouseTo);
                    }
                    //add this transer to the table log movement 
                    var prodMove = new Product_Movement();
                    prodMove.WHouse_from = fromstoreName;
                    prodMove.WHouse_To = toStoreName;
                    prodMove.Product_ID = prodID;
                    prodMove.Production_Date = PDate;
                    prodMove.Transfer_Date = DateTime.Today;
                    prodMove.Quantity = RQuantity;
                    entitiy.Product_Movement.Add(prodMove);
                    entitiy.SaveChanges();
                    //update form
                    fillFromToStoresGridView();
                }
                else
                {
                    MessageBox.Show("Required Transfer quantity isn't Fully available");
                }
            }
            else
            {
                MessageBox.Show("enter the reqyired quantity");
            }
        }

        private void out_product_GV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int reqID = int.Parse(outbound_Customer_GV.SelectedRows[0].Cells[0].Value.ToString());
            int prodID = int.Parse(out_product_GV.SelectedRows[0].Cells[0].Value.ToString());
            DateTime proDate = DateTime.Parse(out_product_GV.SelectedRows[0].Cells[3].Value.ToString());
            var outProd = entitiy.clientRequest_details.Where(r => r.clientRequets_ID == reqID && r.product_ID == prodID && r.Production_date == proDate).Select(r => r).FirstOrDefault();

            out_prodcut_id.Text = prodID.ToString();
            out_prod_date.Text = outProd.Production_date.ToString().Split(' ')[0];
            out_prod_exs.Text = outProd.product.expire_period.ToString();

        }
       // reports dynamic change 
        private void Warehousereport_Click(object sender, EventArgs e)
        {
            if (reportFlag !=0)
            {
                this.reportViewer1.ServerReport.ReportPath = "/Reports/WareHouseReport";
                this.reportViewer1.ServerReport.Refresh();
                this.reportViewer1.RefreshReport();

            }
            else
            {
                reportFlag = 1;
                MessageBox.Show("Please Diploy the Reports to yourlocal Host nd press again\n OR preview in local format ");
            }

        }

        private void PoductsinWH_Click(object sender, EventArgs e)
        {
            if (reportFlag != 0)
            {
                this.reportViewer1.ServerReport.ReportPath = "/Reports/ProductinWareHouse";
                this.reportViewer1.ServerReport.Refresh();
                this.reportViewer1.RefreshReport();

            }
            else
            {
                reportFlag = 1;
                MessageBox.Show("Please Diploy the Reports to yourlocal Host nd press again\n OR preview in local format ");
            }


        }

        private void pro_movement_Click(object sender, EventArgs e)
        {
            if (reportFlag != 0)
            {
                this.reportViewer1.ServerReport.ReportPath = "/Reports/warehouseProductMovement";
                this.reportViewer1.ServerReport.Refresh();
                this.reportViewer1.RefreshReport();

            }
            else
            {
                reportFlag = 1;
                MessageBox.Show("Please Diploy the Reports to yourlocal Host nd press again\n OR preview in local format ");
            }


        }

        private void PeriodinWH_Click(object sender, EventArgs e)
        {
            if (reportFlag != 0)
            {
              // this.reportViewer1.ServerReport.ReportServerUrl = new System.Uri("http://localhost/ReportServer", System.UriKind.Absolute);


                this.reportViewer1.ServerReport.ReportPath = "/Reports/PeriodInWareHouse";
                this.reportViewer1.ServerReport.Refresh();
                this.reportViewer1.RefreshReport();

            }
            else
            {
                reportFlag = 1;
                MessageBox.Show("Please Diploy the Reports to yourlocal Host nd press again\n OR preview in local format ");
            }


        }

        private void Expiresoon_Click(object sender, EventArgs e)
        {
            if (reportFlag != 0)
            {
               // this.reportViewer1.ServerReport.ReportServerUrl = new System.Uri("http://localhost/Reports", System.UriKind.Absolute);

                this.reportViewer1.ServerReport.ReportPath = "/Reports/Expireydate";
                this.reportViewer1.ServerReport.Refresh();
                this.reportViewer1.RefreshReport();

            }
            else
            {
                reportFlag = 1;
                MessageBox.Show("Please Diploy the Reports to yourlocal Host and press again\n OR preview in local format ");
            }


        }

        //this.reportViewer.ServerReport.ReportPath = "/report/Reports/Expireydate";
        //this.reportViewer.ServerReport.ReportServerUrl = new System.Uri("http://yousifwhby/ReportS", System.UriKind.Absolute);
    }
}
