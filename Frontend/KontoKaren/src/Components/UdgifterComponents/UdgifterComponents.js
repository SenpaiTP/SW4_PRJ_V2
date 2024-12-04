import React, { useState } from "react";
import {
  Container,
  Box,
  Paper,
  Table,
  TableContainer,
  IconButton,
  Button,
  TableCell,
  TablePagination
} from "@mui/material";
import { Edit, Delete } from "@mui/icons-material";
import TableBody from "./Table/UdgifterTableBody";
import TableHeader from "./Table/UdgifterTableHeader";
import PieChart from "./PieChart/PieChart";
import "./Table/UdgifterTable.css";
import { initialExpenseRows } from "./Table/UdgifterTableData";
import useUdgifterHooks from "../../Hooks/UdgifterHooks";
import AddExpenseDialog from "./Dialog/AddUdgifterDialog";
import EditExpenseDialog from "./Dialog/EditUdgifterDialog";

export default function UdgifterTable() {
  // bruger hooks til at håndtere logikken i tabellen
  const {
    rows, // data i tabellen
    selected, // valgte rækker
    handleClick, // håndtering af klik på rækker
    handleAddRow, // tilføjelse af en ny række
    handleEditRow, // redigering af en række
    handleDeleteRow, // sletning af en række
    handleSave, // gemmer ændringer
  } = useUdgifterHooks(initialExpenseRows);

  // State for dialog visibility
  const [openAddDialog, setOpenAddDialog] = useState(false); // styrer visningen af dialog for at tilføje udgift
  const [openEditDialog, setOpenEditDialog] = useState(false); // styrer visningen af dialog for at redigere udgift
  const [selectedExpense, setSelectedExpense] = useState(null); // holder den valgte udgift til redigering
  const [selectedCategory, setSelectedCategory] = useState(""); // Holder den valgte kategori
  const [page, setPage] = useState(0); // current page
  const [rowsPerPage, setRowsPerPage] = useState(5); // rows per page

  // Generate data for PieChart
  const chartData = rows.map((row) => ({
    name: row.name, // navn på udgift
    price: row.price, // beløb for udgift
  }));

  // Håndtering af åbning og lukning af dialoger
  const handleClickOpenAdd = () => setOpenAddDialog(true);
  const handleCloseAdd = () => setOpenAddDialog(false);
  const handleClickOpenEdit = (row) => {
    setSelectedExpense(row); // Indstiller den valgte udgift
    setOpenEditDialog(true);
  };
  const handleCloseEdit = () => setOpenEditDialog(false);

  // Tilføjer en ny udgift og lukker dialogen
  const handleAddExpense = (newExpense) => {
    handleAddRow({ ...newExpense, category: selectedCategory });
    setOpenAddDialog(false);
  };

  // Redigerer en eksisterende udgift og lukker dialogen
  const handleEditExpense = (updatedExpense) => {
    handleEditRow({ ...updatedExpense, category: selectedCategory });
    setOpenEditDialog(false);
  };

   // Handle changing pages in pagination
   const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  // Handle changing rows per page
  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0); // Reset to first page when changing rows per page
  };

  return (
    <Container sx={{ display: "flex", paddingLeft: 0, paddingRight: 0 }}>
      {/* venstre side: tabel og knapper */}
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
                    {/* rediger og slet knapper */}
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

             {/* Pagination Controls */}
             <TablePagination
            rowsPerPageOptions={[5, 10, 25]}
            component="div"
            count={rows.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
            labelRowsPerPage="Rækker per side" // Custom label

          />
        </Paper>

        {/* Knapper til at tilføje eller gemme ændringer */}
        <Box sx={{ display: "flex", gap: 2, justifyContent: "flex-start", marginTop: 2 }}>
          <Button variant="contained" onClick={handleClickOpenAdd}>
            Tilføj ny Udgift
          </Button>

          <Button variant="contained" onClick={handleSave}>
            Gem ændringer
          </Button>
        </Box>

        {/* Dialoger til tilføjelse og redigering */}
        <AddExpenseDialog
          open={openAddDialog}
          handleClose={handleCloseAdd}
          handleSave={handleAddExpense}
          selectedCategory={selectedCategory}
          setSelectedCategory={setSelectedCategory}
          />

        <EditExpenseDialog
          open={openEditDialog}
          handleClose={handleCloseEdit}
          handleSave={handleEditExpense}
          expense={selectedExpense}
          selectedCategory={selectedCategory} // Pass selectedCategory here
        />

      </Box>

      {/* højre side: PieChart */}
      <Box sx={{ width: "40%" }}>
        <PieChart chartData={chartData} />
      </Box>
    </Container>
  );
}
