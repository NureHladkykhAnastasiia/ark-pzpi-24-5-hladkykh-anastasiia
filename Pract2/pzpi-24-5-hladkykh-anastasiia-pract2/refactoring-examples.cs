using System;
using System.Data;
using System.Windows.Forms;

namespace CosmeticsShop.Refactoring
{
    // Метод 1: Preserve Whole Object (Збереження цілого об'єкта)
    // До рефакторингу
    public static class PreserveWholeObjectBefore
    {
        public static void EditManufacturer(DataGridView dgvManufacturers, Action reload)
        {
            var row = dgvManufacturers.SelectedRows[0];

            int id = Convert.ToInt32(row.Cells["ID"].Value);
            string name = row.Cells["Назва"].Value?.ToString() ?? "";
            string country = row.Cells["Країна"].Value?.ToString() ?? "";
            string city = row.Cells["Місто"].Value?.ToString() ?? "";
            string address = row.Cells["Адреса"].Value?.ToString() ?? "";
            string certLevel = row.Cells["Рівень сертифікації"].Value?.ToString() ?? "B";
            int capacity = row.Cells["Потужність (тис/рік)"].Value != DBNull.Value
                ? Convert.ToInt32(row.Cells["Потужність (тис/рік)"].Value) : 0;
            int year = row.Cells["Рік заснування"].Value != DBNull.Value
                ? Convert.ToInt32(row.Cells["Рік заснування"].Value) : 0;
            string email = row.Cells["Email"].Value?.ToString() ?? "";

            var form = new ManufacturerForm(id, name, country, city, address,
                certLevel, capacity, year, email);

            if (form.ShowDialog() == DialogResult.OK)
            {
                reload();
            }
        }
    }

    // Після рефакторингу
    public class ManufacturerData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string CertLevel { get; set; }
        public int Capacity { get; set; }
        public int Year { get; set; }
        public string Email { get; set; }

        public static ManufacturerData FromRow(DataGridViewRow row)
        {
            return new ManufacturerData
            {
                Id = Convert.ToInt32(row.Cells["ID"].Value),
                Name = row.Cells["Назва"].Value?.ToString() ?? "",
                Country = row.Cells["Країна"].Value?.ToString() ?? "",
                City = row.Cells["Місто"].Value?.ToString() ?? "",
                Address = row.Cells["Адреса"].Value?.ToString() ?? "",
                CertLevel = row.Cells["Рівень сертифікації"].Value?.ToString() ?? "B",
                Capacity = row.Cells["Потужність (тис/рік)"].Value != DBNull.Value
                    ? Convert.ToInt32(row.Cells["Потужність (тис/рік)"].Value) : 0,
                Year = row.Cells["Рік заснування"].Value != DBNull.Value
                    ? Convert.ToInt32(row.Cells["Рік заснування"].Value) : 0,
                Email = row.Cells["Email"].Value?.ToString() ?? ""
            };
        }
    }

    public static class PreserveWholeObjectAfter
    {
        public static void EditManufacturer(DataGridView dgvManufacturers, Action reload)
        {
            var data = ManufacturerData.FromRow(dgvManufacturers.SelectedRows[0]);
            var form = new ManufacturerForm(data);

            if (form.ShowDialog() == DialogResult.OK)
            {
                reload();
            }
        }
    }


    // Метод 2: Encapsulate Downcast (Інкапсуляція низхідного перетворення типу)
    // До рефакторингу
    public static class EncapsulateDowncastBefore
    {
        public static DataRow FindProductRow(ComboBox cbProduct, int productId)
        {
            var productData = (DataTable)cbProduct.DataSource;
            return productData.Select($"product_id = {productId}")[0];
        }
    }

    // Після рефакторингу
    public static class EncapsulateDowncastAfter
    {
        public static DataTable GetProductTable(ComboBox cbProduct)
        {
            return cbProduct.DataSource as DataTable;
        }

        public static DataRow FindProductRow(ComboBox cbProduct, int productId)
        {
            DataTable productData = GetProductTable(cbProduct);
            if (productData == null)
            {
                MessageBox.Show("Список товарів не завантажено!", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            DataRow[] found = productData.Select($"product_id = {productId}");
            return found.Length > 0 ? found[0] : null;
        }
    }


    // Метод 3: Form Template Method (Формування шаблонного методу)
    // До рефакторингу
    public static class FormTemplateMethodBefore
    {
        public static void DeleteManufacturer(DataGridView dgvManufacturers, Action reload)
        {
            var db = new DatabaseHelper();

            if (dgvManufacturers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть виробника для видалення!", "Увага",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = dgvManufacturers.SelectedRows[0].Cells["Назва"].Value.ToString();
            var result = MessageBox.Show($"Видалити виробника: {name}?",
                "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvManufacturers.SelectedRows[0].Cells["ID"].Value);
                db.ExecuteNonQuery($"DELETE FROM Manufacturers WHERE manufacturer_id = {id}");
                MessageBox.Show("Виробника видалено!", "Успіх",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                reload();
            }
        }

        public static void DeleteWarehouse(DataGridView dgvWarehouses, Action reload)
        {
            var db = new DatabaseHelper();

            if (dgvWarehouses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть склад для видалення!", "Увага",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = dgvWarehouses.SelectedRows[0].Cells["Назва складу"].Value.ToString();
            var result = MessageBox.Show($"Видалити склад: {name}?",
                "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvWarehouses.SelectedRows[0].Cells["ID"].Value);
                db.ExecuteNonQuery($"DELETE FROM Warehouses WHERE warehouse_id = {id}");
                MessageBox.Show("Склад видалено!", "Успіх",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                reload();
            }
        }
    }

    // Після рефакторингу
    public abstract class ReferenceDeleteTemplate
    {
        protected readonly DatabaseHelper db = new DatabaseHelper();

        protected abstract DataGridView Grid { get; }
        protected abstract string EntityGenitive { get; }
        protected abstract string EntityNominative { get; }
        protected abstract string NameColumn { get; }
        protected abstract string TableName { get; }
        protected abstract string IdColumn { get; }
        protected abstract void Reload();

        public void DeleteSelected()
        {
            if (Grid.SelectedRows.Count == 0)
            {
                MessageBox.Show($"Виберіть {EntityGenitive} для видалення!", "Увага",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = Grid.SelectedRows[0].Cells[NameColumn].Value?.ToString() ?? "";
            var confirm = MessageBox.Show($"Видалити {EntityGenitive}: {name}?",
                "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            int id = GetSelectedId();
            db.ExecuteNonQuery($"DELETE FROM {TableName} WHERE {IdColumn} = {id}");
            MessageBox.Show($"{EntityNominative} видалено!", "Успіх",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            Reload();
        }

        protected int GetSelectedId()
        {
            return Convert.ToInt32(Grid.SelectedRows[0].Cells["ID"].Value);
        }
    }

    public class ManufacturerDeleter : ReferenceDeleteTemplate
    {
        private readonly DataGridView grid;
        private readonly Action reload;

        public ManufacturerDeleter(DataGridView grid, Action reload)
        {
            this.grid = grid;
            this.reload = reload;
        }

        protected override DataGridView Grid => grid;
        protected override string EntityGenitive => "виробника";
        protected override string EntityNominative => "Виробник";
        protected override string NameColumn => "Назва";
        protected override string TableName => "Manufacturers";
        protected override string IdColumn => "manufacturer_id";
        protected override void Reload() => reload();
    }

    public class WarehouseDeleter : ReferenceDeleteTemplate
    {
        private readonly DataGridView grid;
        private readonly Action reload;

        public WarehouseDeleter(DataGridView grid, Action reload)
        {
            this.grid = grid;
            this.reload = reload;
        }

        protected override DataGridView Grid => grid;
        protected override string EntityGenitive => "склад";
        protected override string EntityNominative => "Склад";
        protected override string NameColumn => "Назва складу";
        protected override string TableName => "Warehouses";
        protected override string IdColumn => "warehouse_id";
        protected override void Reload() => reload();
    }
}
