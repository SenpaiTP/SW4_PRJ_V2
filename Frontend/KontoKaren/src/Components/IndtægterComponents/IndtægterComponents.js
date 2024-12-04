import React, { useState } from "react"; 
import { Container, Box, Paper, Table, TableContainer, IconButton, Button, TableCell, TablePagination} from "@mui/material"; 
import { Edit, Delete } from "@mui/icons-material"; 
import TableBody from "./Table/IndtægterTableBody"; 
import TableHeader from "./Table/IndtægterTableHeader"; 
import PieChart from "./PieChart/PieChart"; 
import './Table/IndtægterTable.css'
import { initialRows } from "./Table/IndtægterTableData"; 
import AddIncomeDialog from "./Dialog/AddIndtægterDialog"; 
import EditIncomeDialog from "./Dialog/EditIndtægterDialog"; 
import useIndtægterHooks from "../../Hooks/IndtægterHooks"; 

export default function IndtægterTable() {
  // bruger hooks til at håndtere logikken i tabellen
  const {
    rows, // data i tabellen
    selected, // valgte rækker
    handleClick, // håndtering af klik på rækker
    handleAddRow, // tilføjelse af en ny række
    handleEditRow, // redigering af en række
    handleDeleteRow, // sletning af en række
    handleSave, // gemmer ændringer
  } = useIndtægterHooks(initialRows);

  const [openAddDialog, setOpenAddDialog] = useState(false); // styrer visningen af dialog for at tilføje indkomst
  const [openEditDialog, setOpenEditDialog] = useState(false); // styrer visningen af dialog for at redigere indkomst
  const [selectedIncome, setSelectedIncome] = useState(null); // holder den valgte indkomst til redigering
  const [page, setPage] = useState(0); // current page
  const [rowsPerPage, setRowsPerPage] = useState(5); // rows per page

  // ppretter data til PieChart
  const chartData = rows.map((row) => ({
    name: row.name, // navn på indtægt
    price: row.price, // bløb for indtægt
  }));

  // håndtering af åbning og lukning af dialoger
  const handleClickOpenAdd = () => setOpenAddDialog(true);
  const handleCloseAdd = () => setOpenAddDialog(false);
  const handleClickOpenEdit = (row) => {
    setSelectedIncome(row); // Indstiller den valgte indkomst
    setOpenEditDialog(true);
  };
  const handleCloseEdit = () => setOpenEditDialog(false);

  // Tilføjer en ny indkomst og lukker dialogen
  const handleAddIncome = (newIncome) => {
    handleAddRow(newIncome);
    setOpenAddDialog(false);
  };

  // Redigerer en eksisterende indkomst og lukker dialogen
  const handleEditIncome = (updatedIncome) => {
    handleEditRow(updatedIncome);
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

        {/* knapper til at tilføje eller gemme ændringer */}
        <Box sx={{ display: "flex", gap: 2, justifyContent: "flex-start", marginTop: 2 }}>
          <Button
            variant="contained"
            onClick={handleClickOpenAdd}
          >
            Tilføj ny Indtægt
          </Button>

          <Button
            variant="contained"
            onClick={handleSave}
          >
            Gem ændringer
          </Button>
        </Box>

        {/* dialoger til tilføjelse og redigering */}
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
      </Box>

      {/* højre side: PieChart */}
      <Box sx={{ width: "40%" }}>
        <PieChart chartData={chartData} />
      </Box>
    </Container>
  );
}
