
// Function to create data
export function createData(id, name, price, date) {
  return { id, name, price, date };
}

// Initial data example
export const initialRows = [
  createData(1, "SU", 305, "2022-12-01"),
  createData(2, "Løn", 452, "2022-12-01"),
  createData(3, "Sort arbejde", 250, "2022-12-01"),
];
