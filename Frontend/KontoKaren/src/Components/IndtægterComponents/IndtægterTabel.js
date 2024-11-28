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
import { initialRows } from "./Table/TableData";

import AddIncomeDialog from "./Dialog/AddIncomeDialog";
import EditIncomeDialog from "./Dialog/EditIncomeDialog";  // Import EditIncomeDialog
import useIndtægterHooks from "../../Hooks/IndtægterHooks";

export default function IndtægterTabel() {
  const {
    rows,
    selected,
    page,
    rowsPerPage,
    handleClick,
    handleAddRow,
    handleEditRow,
    handleDeleteRow,
    handleSave,
  } = useIndtægterHooks(initialRows);

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
