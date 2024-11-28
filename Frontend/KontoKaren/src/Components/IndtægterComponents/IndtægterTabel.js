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
import TableHeader from "./Table/TableHeader";
import PieChart from "./PieChart/PieChart";
import { createData, initialRows } from "./Table/TableData";

import AddIncomeDialog from "./Dialog/AddIncomeDialog";
import EditIncomeDialog from "./Dialog/EditIncomeDialog";  // Import EditIncomeDialog

function useIntægterHooks(initialRows) {
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
    handleEditRow,
    handleDeleteRow,
    handleSave,
    handleAddRow, // vi eksponerer handleAddRow her
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

  const [openAddDialog, setOpenAddDialog] = useState(false);
  const [openEditDialog, setOpenEditDialog] = useState(false);
  const [selectedIncome, setSelectedIncome] = useState(null);  // Store selected income for editing

  const chartData = rows.map((row) => ({
    name: row.name,
    price: row.price,
  }));

  const handleClickOpenAdd = () => setOpenAddDialog(true);
  const handleCloseAdd = () => setOpenAddDialog(false);

  const handleClickOpenEdit = (row) => {
    setSelectedIncome(row);  // Set the income to be edited
    setOpenEditDialog(true);
  };
  const handleCloseEdit = () => setOpenEditDialog(false);

  const handleAddIncome = (newIncome) => {
    handleAddRow(newIncome);
    setOpenAddDialog(false);
  };

  const handleEditIncome = (updatedIncome) => {
    handleEditRow(updatedIncome);
    setOpenEditDialog(false);
  };

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
                  <IconButton onClick={() => handleClickOpenEdit(row)}>  
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
        onClick={handleClickOpenAdd}  // This should trigger handleClickOpenAdd
        sx={{ marginTop: 2, marginRight: 2 }}
      >
        Tilføj ny Indtægt
      </Button>

      <Button
        variant="contained"
        onClick={handleSave}
        sx={{ marginTop: 2, marginRight: 2 }}
      >
        Gem
      </Button>

      <AddIncomeDialog
        open={openAddDialog}
        handleClose={handleCloseAdd}
        handleSave={handleAddIncome}
      />

      <EditIncomeDialog
        open={openEditDialog}
        handleClose={handleCloseEdit}
        handleSave={handleEditIncome}
        income={selectedIncome}
      />

      <PieChart chartData={chartData} />
    </Box>
  );
}
