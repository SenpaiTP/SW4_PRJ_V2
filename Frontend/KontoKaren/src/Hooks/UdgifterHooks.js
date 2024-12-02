import { useState, useEffect } from "react";
import { createData } from "../Components/UdgifterComponents/Table/UdgifterTableData";

export default function useUdgifterHooks(initialExpenseRows) {
    const [rows, setRows] = useState(initialExpenseRows);
    const [selected, setSelected] = useState([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);

    const handleAddRow = (newExpense) => {
      const newRow = createData(
        Date.now(),
        //rows.length + 1,
        newExpense.name,
        newExpense.category,
        newExpense.price,
        newExpense.date
      );
      setRows([...rows, newRow]);
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
      localStorage.setItem("rows", JSON.stringify(rows));
      alert ("Ændringer er gemt") 
    };

    useEffect(() => {
        const savedRows = JSON.parse(localStorage.getItem("rows")) || initialExpenseRows;
        setRows(savedRows);
      }, [initialExpenseRows]);  // Added initialExpenseRows as a dependency
      
  
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
  