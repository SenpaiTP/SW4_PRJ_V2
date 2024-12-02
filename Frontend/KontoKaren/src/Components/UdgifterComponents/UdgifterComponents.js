import React, { useState } from "react"; 
import { Container, Box, Paper, Table, TableContainer, IconButton, Button, TableCell } from "@mui/material"; 
import { Edit, Delete } from "@mui/icons-material"; 
import TableBody from "./Table/UdgifterTableBody"; 
import TableHeader from "./Table/UdgifterTableHeader"; 
import PieChart from "./PieChart/PieChart"; 
import './Table/UdgifterTable.css'; 
import { initialExpenseRows } from "./Table/UdgifterTableData"; 
import useUdgifterHooks from "../../Hooks/UdgifterHooks";
import AddExpenseDialog from "./Dialog/AddUdgifterDialog";
import EditExpenseDialog from "./Dialog/EditUdgifterDialog";

export default function UdgifterTable() {
  // bruger hooks til at håndtere logikken i tabellen
  const {
    rows, // data i tabellen
    selected, // valgte rækker
    page, // sktuel side
    rowsPerPage, // antal rækker pr. side
    handleClick, // håndtering af klik på rækker
    handleAddRow, // tilføjelse af en ny række
    handleEditRow, // redigering af en række
    handleDeleteRow, // sletning af en række
    handleSave, // gemmer ændringer
  } = useUdgifterHooks(initialExpenseRows);

  const [openAddDialog, setOpenAddDialog] = useState(false); // styrer visningen af dialog for at tilføje indkomst
  const [openEditDialog, setOpenEditDialog] = useState(false); // styrer visningen af dialog for at redigere indkomst
  const [selectedExpense, setSelectedExpense] = useState(null); // holder den valgte indkomst til redigering

  // ppretter data til PieChart
  const chartData = rows.map((row) => ({
    name: row.name, // navn på udgift
    price: row.price, // bløb for udgift
  }));

  // håndtering af åbning og lukning af dialoger
  const handleClickOpenAdd = () => setOpenAddDialog(true);
  const handleCloseAdd = () => setOpenAddDialog(false);
  const handleClickOpenEdit = (row) => {
    setSelectedExpense(row); // Indstiller den valgte indkomst
    setOpenEditDialog(true);
  };
  const handleCloseEdit = () => setOpenEditDialog(false);

  // Tilføjer en ny indkomst og lukker dialogen
  const handleAddExpense = (newExpense) => {
    handleAddRow(newExpense);
    setOpenAddDialog(false);
  };

  // Redigerer en eksisterende indkomst og lukker dialogen
  const handleEditExpense = (updatedExpense) => {
    handleEditRow(updatedExpense);
    setOpenEditDialog(false);
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
        </Paper>

        {/* knapper til at tilføje eller gemme ændringer */}
        <Box sx={{ display: "flex", gap: 2, justifyContent: "flex-start", marginTop: 2 }}>
          <Button
            variant="contained"
            onClick={handleClickOpenAdd}
          >
            Tilføj ny Udgift
          </Button>

          <Button
            variant="contained"
            onClick={handleSave}
          >
            Gem ændringer
          </Button>
        </Box>

        {/* dialoger til tilføjelse og redigering */}
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

      {/* højre side: PieChart */}
      <Box sx={{ width: "40%" }}>
        <PieChart chartData={chartData} />
      </Box>
    </Container>
  );
}
