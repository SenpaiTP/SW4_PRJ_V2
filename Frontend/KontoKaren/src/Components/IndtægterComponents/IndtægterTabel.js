import React, { useState } from "react";
import {
  Box,
  Paper,
  Table,
  TableContainer,
  IconButton,
  Button,
  TableCell,
} from "@mui/material";
import { Edit, Delete } from "@mui/icons-material";

import TableBody from "./Table/TableBody";
import TableHeader from "./PieChart/TableHeader";
import PieChart from "./PieChart/PieChart";
import { createData, initialRows } from "./Table/TableData";

// useIntægterHooks hooken
function useIntægterHooks(initialRows) {
  const [rows, setRows] = useState(initialRows);
  const [selected, setSelected] = useState([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [newRow, setNewRow] = useState({
    id: "",
    name: "",
    price: "",
    date: "",
  });

  const chartData = rows.map((row) => ({
    name: row.name,
    price: row.price,
  }));

  const handleAddRow = () => {
    const newRow = createData(
      rows.length + 1,
      prompt("Indtægtsnavn:"),
      prompt("Pris:"),
      prompt("Dato (YYYY-MM-DD):")
    );
    setRows([...rows, newRow]);
  };

  const handleEditRow = (row) => {
    const updatedName = prompt("Rediger navn:", row.name);
    const updatedPrice = prompt("Rediger pris:", row.price);
    const updatedDate = prompt("Rediger dato (YYYY-MM-DD):", row.date);

    if (updatedName && updatedPrice && updatedDate) {
      setRows((prevRows) =>
        prevRows.map((r) =>
          r.id === row.id
            ? {
                ...r,
                name: updatedName,
                price: updatedPrice,
                date: updatedDate,
              }
            : r
        )
      );
    }
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
    alert("Ændringerne er gemt!");
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
    handleAddRow,
    handleEditRow,
    handleDeleteRow,
    handleSave,
  };
}

export default function IndtægterTabel() {
  const {
    rows,
    selected,
    page,
    rowsPerPage,
    handleClick,
    handleChangePage,
    handleChangeRowsPerPage,
    handleAddRow,
    handleEditRow,
    handleDeleteRow,
    handleSave,
  } = useIntægterHooks(initialRows);

  const chartData = rows.map((row) => ({
    name: row.name,
    price: row.price,
  }));

  return (
    <Box sx={{ width: "100%" }}>
      <Paper sx={{ width: "100%", mb: 2, position: "relative" }}>
        <TableContainer>
          <Table>
            <TableHeader numSelected={selected.length} rowCount={rows.length} />
            <TableBody
              rows={rows}
              selected={selected}
              page={page}
              rowsPerPage={rowsPerPage}
              onRowClick={handleClick}
              renderActions={(row) => (
                <TableCell>
                  <IconButton onClick={() => handleEditRow(row)}>
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleDeleteRow(row.id)}>
                    <Delete />
                  </IconButton>
                </TableCell>
              )}
            />
          </Table>
        </TableContainer>
      </Paper>

      <Button
        variant="contained"
        aligned="right"
        onClick={handleAddRow}
        sx={{ marginTop: 2, marginRight: 2 }}
      >
        Tilføj ny Indtægt
      </Button>

      <Button
        variant="contained"
        aligned="right"
        onClick={handleSave}
        sx={{ marginTop: 2, marginRight: 2 }}
      >
        Gem
      </Button>

      <PieChart chartData={chartData} />
    </Box>
  );
}
