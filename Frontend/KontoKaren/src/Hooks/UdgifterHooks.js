import { useState, useEffect } from "react";
import { createData } from "../Utils/CreateData";

export default function useUdgifterHooks(initialExpenseRows) {
    const [rows, setRows] = useState(initialExpenseRows);
    const [selected, setSelected] = useState([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);

    useEffect(() => {
        const savedRows = JSON.parse(localStorage.getItem("udgifterRows")) || initialExpenseRows;
        setRows(savedRows);
    }, [initialExpenseRows]);

    const handleAddRow = (newExpense) => {
      const newRow = createData (
        Date.now(),
        newExpense.name,
        newExpense.category,
        newExpense.price,
        newExpense.date
      );
      setRows([...rows, newRow]);
      console.log("After adding row:", [...rows, newRow]);
    };

    const handleEditRow = (newExpense) => {
      setRows((prevRows) =>
        prevRows.map((r) =>
          r.id === newExpense.id
            ? { ...r, name: newExpense.name, category: newExpense.category, price: newExpense.price, date: newExpense.date }
            : r
        )
      );
      console.log("After editing row:", rows);
    };

    const handleDeleteRow = (id) => {
      setRows((prevRows) => prevRows.filter((row) => row.id !== id));
      console.log("After deleting row:", rows);
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
      localStorage.setItem("udgifterRows", JSON.stringify(rows));
      alert("Ændringer er gemt");
    };

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
    };
}