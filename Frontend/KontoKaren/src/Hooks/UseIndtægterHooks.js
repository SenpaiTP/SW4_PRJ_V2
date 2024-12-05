import { useState, useEffect } from "react";
import { createData } from "../Utils/CreateData";

export default function useIndtægterHooks(initialRows) {
    const [rows, setRows] = useState(initialRows);
    const [selected, setSelected] = useState([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);

    useEffect(() => {
        const savedRows = JSON.parse(localStorage.getItem("indtægterRows")) || initialRows;
        setRows(savedRows);
    }, [initialRows]);

    const handleAddRow = (newIncome) => {
      const newRow = createData (
        Date.now(),
        newIncome.name,
        newIncome.price,
        newIncome.date
      );
      setRows((prevRows) => [newRow, ...prevRows]); // Tilføj den nye række øverst
      console.log("After adding row:", [...rows, newRow]);
    };

    const handleEditRow = (newIncome) => {
      setRows((prevRows) =>
        prevRows.map((r) =>
          r.id === newIncome.id
            ? { ...r, name: newIncome.name, price: newIncome.price, date: newIncome.date }
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
      localStorage.setItem("indtægterRows", JSON.stringify(rows));
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