
// Function to create data
export function createData(id, name, category, price, date) //PLEASE FOR THE LOVE OF GOD FUCKING PUT TYPES ON UR PARAMETERS (GÆLDER FOR ALLE STEDER I DIN KODE JULIE!!!!)
 {
    return { id, name, category, price, date };
  }
  
  // Initial data example
  export const initialExpenseRows = [
    createData(1, "Bukser", "Tøj", 305, "2022-12-01"),
    createData(2, "Ærter", "Mad", 12, "2022-12-01"),
    createData(3, "Tivoli", "Fornøjelser", 250, "2022-12-01"),
  ];
  