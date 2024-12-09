import { useState, useEffect } from "react";
import { createData } from "../Utils/CreateData";

export default function useUdgifterHooks(initialExpenseRows, storageKey) {
  const [rows, setRows] = useState(initialExpenseRows);
  const [selected, setSelected] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [category, setCategory] = useState(localStorage.getItem(`${storageKey}-category`) || ''); // Corrected with backticks

  useEffect(() => {
    const savedRows = JSON.parse(localStorage.getItem(storageKey)) || initialExpenseRows;
    setRows(savedRows);
  }, [initialExpenseRows, storageKey]);

  useEffect(() => {
    if (category) {
      localStorage.setItem(`${storageKey}-category`, category); // Corrected with backticks
    }
  }, [category, storageKey]);

  const addExpense = (expense) => {
    setRows((prevRows) => [...prevRows, expense]);
    setCategory(expense.category); // Update category when adding an expense
  };

  const handleAddRow = (newExpense) => {
    const newRow = createData(
      Date.now(),
      newExpense.name,
      newExpense.category,
      newExpense.price,
      newExpense.date
    );
    setRows((prevRows) => [newRow, ...prevRows]); // Add the new row at the top
  };

  const handleEditRow = (newExpense) => {
    setRows((prevRows) =>
      prevRows.map((r) =>
        r.id === newExpense.id
          ? { ...r, name: newExpense.name, category: newExpense.category, price: newExpense.price, date: newExpense.date }
          : r
      )
    );
  };

  const handleDeleteRow = (id) => {
    setRows((prevRows) => prevRows.filter((row) => row.id !== id));
  };

  const handleClick = (event, id) => {
    const selectedIndex = selected.indexOf(id);
    let newSelected = [];

    if (selectedIndex === -1) {
      newSelected = newSelected.concat(selected, id);
    } else {
      newSelected = newSelected.concat(
        selected.slice(0, selectedIndex),
        selected.slice(selectedIndex + 1)
      );
    }
    setSelected(newSelected);
  };

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleSave = () => {
    localStorage.setItem(storageKey, JSON.stringify(rows)); // Store rows with unique storageKey
    alert("Ændringer er gemt");
  };

  // Log the updated category whenever it changes
  useEffect(() => {
    console.log("Updated category:", category);
  }, [category]);

  return {
    rows,
    setRows,
    selected,
    setSelected,
    page,
    setPage,
    rowsPerPage,
    setRowsPerPage,
    handleClick,
    handleChangePage,
    handleChangeRowsPerPage,
    handleEditRow,
    handleDeleteRow,
    handleSave,
    handleAddRow,
    category,
    setCategory,
    addExpense
  };
}
