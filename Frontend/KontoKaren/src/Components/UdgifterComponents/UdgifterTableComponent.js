import React, { useState } from 'react';
import { Container, Box, Paper, Table, TableContainer, IconButton, Button, TableCell, Typography, TablePagination } from '@mui/material';
import { Edit, Delete } from "@mui/icons-material";
import TableBody from "./Table/UdgifterTableBody";
import TableHeader from "./Table/UdgifterTableHeader";
//import PieChartColors from "./PieChartColors";
import PieChartColors from './PieChart/PieChartColors';
import './Table/UdgifterTable.css';
import useUdgifterHooks from "../../Hooks/UseUdgifterHooks";
import AddExpenseDialog from "./Dialog/AddUdgifterDialog";
import EditExpenseDialog from "./Dialog/EditUdgifterDialog";

export default function UdgifterTableComponent({ title, initialRows, storageKey, fetchFunction, createFunction, updateFunction, deleteFunction }) {
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
    handleChangePage,
    handleChangeRowsPerPage,
  } = useUdgifterHooks(initialRows, storageKey, fetchFunction, createFunction, updateFunction, deleteFunction);

  const [openAddDialog, setOpenAddDialog] = useState(false);
  const [openEditDialog, setOpenEditDialog] = useState(false);
  const [selectedExpense, setSelectedExpense] = useState(null);

  const handleClickOpenAdd = () => setOpenAddDialog(true);
  const handleCloseAdd = () => setOpenAddDialog(false);
  const handleClickOpenEdit = (row) => {
    setSelectedExpense(row);
    setOpenEditDialog(true);
  };
  const handleCloseEdit = () => setOpenEditDialog(false);

  const handleAddExpense = (newExpense) => {
    handleAddRow(newExpense);
    setOpenAddDialog(false);
  };

  const handleEditExpense = (updatedExpense) => {
    handleEditRow(updatedExpense);
    setOpenEditDialog(false);
  };

  const chartData = rows.map((row) => ({
    name: row.name,
    price: row.price,
  }));

  return (
    <Container sx={{ display: "flex", paddingLeft: 0, paddingRight: 0 }}>
      <Box sx={{ width: "60%", paddingRight: 2 }}>
        <Paper sx={{ width: "100%", mb: 2, position: "relative", marginTop: 6 }}>
          <Typography variant="h6" sx={{ marginTop: 6, marginBottom: 1, textAlign: "center" }}>
            {title}
          </Typography>
          <TableContainer>
            <Table className="table">
              <TableHeader numSelected={selected.length} rowCount={rows.length} />
              <TableBody
                rows={rows}
                selected={selected}
                page={page}
                rowsPerPage={rowsPerPage}
                onRowClick={handleClick}
                renderActions={(row) => (
                  <TableCell>
                    <Box sx={{ display: "flex", gap: 1 }}>
                      <IconButton onClick={() => handleClickOpenEdit(row)}>
                        <Edit />
                      </IconButton>
                      <IconButton onClick={() => handleDeleteRow(row.id)}>
                        <Delete />
                      </IconButton>
                    </Box>
                  </TableCell>
                )}
              />
            </Table>
          </TableContainer>
          <TablePagination
            rowsPerPageOptions={[5, 10, 25]}
            component="div"
            count={rows.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
            labelRowsPerPage="Rækker per side"
          />
        </Paper>
        <Box sx={{ display: "flex", gap: 2, justifyContent: "flex-start", marginTop: 2 }}>
          <Button variant="contained" onClick={handleClickOpenAdd}>
            Tilføj ny Udgift
          </Button>
          <Button variant="contained" onClick={handleSave}>
            Gem ændringer
          </Button>
        </Box>
        <AddExpenseDialog
          open={openAddDialog}
          handleClose={handleCloseAdd}
          handleSave={handleAddExpense}
        />
        <EditExpenseDialog
          open={openEditDialog}
          handleClose={handleCloseEdit}
          handleSave={handleEditExpense}
          expense={selectedExpense}
        />
      </Box>
      <Box sx={{ width: "40%" }}>
        <PieChartColors chartData={chartData} />
      </Box>
    </Container>
  );
}