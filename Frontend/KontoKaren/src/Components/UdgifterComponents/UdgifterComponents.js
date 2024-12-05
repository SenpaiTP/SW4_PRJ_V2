import React, { useState } from "react";
import { Container, Box, Paper, Table, TableContainer, IconButton, Button, TableCell, TablePagination } from "@mui/material";
import { Edit, Delete } from "@mui/icons-material";
import TableBody from "./Table/UdgifterTableBody";
import TableHeader from "./Table/UdgifterTableHeader";
import PieChart from "./PieChart/PieChart";
import './Table/UdgifterTable.css';
import { initialExpenseRows } from "./Table/UdgifterTableData";
import useUdgifterHooks from "../../Hooks/UseUdgifterHooks";
import AddExpenseDialog from "./Dialog/AddUdgifterDialog";
import EditExpenseDialog from "./Dialog/EditUdgifterDialog";
import SuggestCategory from "../../Services/CategoryService";

export default function UdgifterTable() {
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
  } = useUdgifterHooks(initialExpenseRows);

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



  

  rows.forEach((row) => {
    const descriptionName = { description: row.name };
    SuggestCategory(descriptionName.description).then((category) => {
        if (category) {
            console.log(`Navn: ${row.name}, Foreslået kategori: ${category}`);
        }
    });
});


  return (
    <Container sx={{ display: "flex", paddingLeft: 0, paddingRight: 0 }}>
      <Box sx={{ width: "60%", paddingRight: 2 }}>
        <Paper sx={{ width: "100%", mb: 2, position: "relative", marginTop: 6 }}>
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
        <PieChart chartData={chartData} />
      </Box>
    </Container>
  );
}