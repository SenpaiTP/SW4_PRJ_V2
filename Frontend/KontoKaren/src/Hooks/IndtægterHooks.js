import { useState, useEffect } from "react";
import { createData } from "../Components/IndtægterComponents/Table/TableData";


export default function useIndtægterHooks(initialRows) {
    const [rows, setRows] = useState(initialRows);
    const [selected, setSelected] = useState([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);

    const handleAddRow = (newIncome) => {
      const newRow = createData(
        rows.length + 1,
        newIncome.name,
        newIncome.price,
        newIncome.date
      );
      setRows([...rows, newRow]);
    };
  
    const handleEditRow = (newIncome) => {
      setRows((prevRows) =>
        prevRows.map((r) =>
          r.id === newIncome.id
            ? { ...r, name: newIncome.name, price: newIncome.price, date: newIncome.date }
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
      console.log("Saving rows:", rows);  // Debugging line to check data
      localStorage.setItem("rows", JSON.stringify(rows));
      alert ("Ændringer er gemt") 
    };

    useEffect(() => {
      const savedRows = JSON.parse(localStorage.getItem("rows")) || [];
      setRows(savedRows); // Opdaterer `rows` med gemte data
    }, []);
  
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
  