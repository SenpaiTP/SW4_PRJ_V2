// Function to create data
  export function createData({ id, name, category = "Unknown", price, date }) {
    if (typeof id !== "number") throw new Error("id must be a number");
    if (typeof name !== "string") throw new Error("name must be a string");
    if (typeof category !== "string") throw new Error("category must be a string");
    if (typeof price !== "number") throw new Error("price must be a number");
    if (typeof date !== "string") throw new Error("date must be a string");
  
    return { id, name, category, price, date };
  }
  