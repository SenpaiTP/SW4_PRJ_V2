import * as React from "react";
import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Checkbox,
  IconButton,
  Menu,
  MenuItem,
  TablePagination,
} from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import PieChart from "./PieChart";

// Funktion til at oprette data
function createData(id, name, price, date) {
  return { id, name, price, date };
}

// Eksempeldata
const initialRows = [
  createData(1, "SU", 305, "2022-12-01"),
  createData(2, "Løn", 452, "2022-12-01"),
  createData(3, "Sort arbejde", 250, "2022-12-01"),
];

export default function IndtægterTabel() {
  const [rows, setRows] = React.useState(initialRows);
  const [anchorEl, setAnchorEl] = React.useState(null);
  const [selected, setSelected] = React.useState([]);
  const [selectedRow, setSelectedRow] = React.useState(null); // Gemmer den valgte række
  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(5);

  const handleClickMenu = (event, row) => {
    setAnchorEl(event.currentTarget); // Åbner menuen
    setSelectedRow(row); // Gem den række, der er knyttet til denne menu
  };

  const handleCloseMenu = () => {
    setAnchorEl(null); // Lukker menuen
    setSelectedRow(null); // Rydder valgt række
  };

  const handleAddRow = () => {
    const newRow = createData(
      rows.length + 1,
      prompt("Indtægtsnavn:"),
      prompt("Pris:"),
      prompt("Dato (YYYY-MM-DD):")
    );
    setRows([...rows, newRow]); // Tilføjer en ny række
    handleCloseMenu(); // Lukker menuen
  };

  const handleEditRow = () => {
    if (selectedRow) {
      const newName = prompt("Rediger navn:", selectedRow.name);
      const newPrice = prompt("Rediger pris:", selectedRow.price);
      const newDate = prompt("Rediger dato (YYYY-MM-DD):", selectedRow.date);

      if (newName && newPrice && newDate) {
        setRows((prevRows) =>
          prevRows.map((row) =>
            row.id === selectedRow.id
              ? {
                  ...row,
                  name: newName,
                  price: parseFloat(newPrice),
                  date: newDate,
                }
              : row
          )
        );
      }
    }
    handleCloseMenu(); // Lukker menuen
  };

  const handleDeleteRow = () => {
    if (selectedRow) {
      setRows(rows.filter((row) => row.id !== selectedRow.id)); // Slet den valgte række
    }
    handleCloseMenu(); // Lukker menuen
  };

  const handleSelectAllClick = (event) => {
    if (event.target.checked) {
      setSelected(rows.map((n) => n.id));
    } else {
      setSelected([]);
    }
  };

  const handleClick = (event, id) => {
    const selectedIndex = selected.indexOf(id);
    let newSelected = [];

    if (selectedIndex === -1) {
      newSelected = newSelected.concat(selected, id);
    } else if (selectedIndex === 0) {
      newSelected = newSelected.concat(selected.slice(1));
    } else if (selectedIndex === selected.length - 1) {
      newSelected = newSelected.concat(selected.slice(0, -1));
    } else if (selectedIndex > 0) {
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

  const chartData = rows.map((row) => ({
    name: row.name,
    price: row.price,
  }));

  return (
    <Box sx={{ width: "100%" }}>
      <Paper sx={{ width: "100%", mb: 2, position: "relative" }}>
        <IconButton
          onClick={handleClickMenu}
          sx={{
            position: "absolute", // Placeres absolut i forhold til Paper
            zIndex: 1,
            top: 10, // Afstand fra toppen af Paper
            right: 10, // Afstand fra højre side af Paper
          }}
        >
          <MoreVertIcon />
        </IconButton>

        <TableContainer>
          <Table sx={{ minWidth: 750 }} aria-labelledby="tableTitle">
            <TableHead>
              <TableRow>
                <TableCell padding="checkbox">
                  <Checkbox
                    color="primary"
                    indeterminate={
                      selected.length > 0 && selected.length < rows.length
                    }
                    checked={rows.length > 0 && selected.length === rows.length}
                    onChange={handleSelectAllClick}
                  />
                </TableCell>
                <TableCell>Indtægtsnavn</TableCell>
                <TableCell align="right">Pris</TableCell>
                <TableCell align="right">Dato</TableCell>
              </TableRow>
            </TableHead>

            <TableBody>
              {rows
                .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                .map((row) => {
                  const isItemSelected = selected.indexOf(row.id) !== -1;
                  return (
                    <TableRow
                      hover
                      onClick={(event) => handleClick(event, row.id)}
                      role="checkbox"
                      aria-checked={isItemSelected}
                      tabIndex={-1}
                      key={row.id}
                      selected={isItemSelected}
                    >
                      <TableCell padding="checkbox">
                        <Checkbox color="primary" checked={isItemSelected} />
                      </TableCell>
                      <TableCell>{row.name}</TableCell>
                      <TableCell align="right">{row.price}</TableCell>
                      <TableCell align="right">{row.date}</TableCell>
                      <TableCell align="center"></TableCell>
                    </TableRow>
                  );
                })}
            </TableBody>
          </Table>
        </TableContainer>

        <Menu
          anchorEl={anchorEl}
          open={Boolean(anchorEl)}
          onClose={handleCloseMenu}
        >
          <MenuItem onClick={handleAddRow}>Tilføj indtægt</MenuItem>
          <MenuItem onClick={handleEditRow}>Rediger indtægt</MenuItem>
          <MenuItem onClick={handleDeleteRow}>Slet indtægt</MenuItem>
        </Menu>

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

      <PieChart chartData={chartData} />
    </Box>
  );
}
