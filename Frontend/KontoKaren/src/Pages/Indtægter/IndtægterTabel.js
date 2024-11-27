import * as React from "react";
import PropTypes from "prop-types";
import { alpha } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TablePagination from "@mui/material/TablePagination";
import TableRow from "@mui/material/TableRow";
import TableSortLabel from "@mui/material/TableSortLabel";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import Paper from "@mui/material/Paper";
import Checkbox from "@mui/material/Checkbox";
import IconButton from "@mui/material/IconButton";
import Tooltip from "@mui/material/Tooltip";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import { visuallyHidden } from "@mui/utils";
import PieChart from "./PieChart";

// Funktion til at oprette data
function createData(id, name, price, category) {
  return { id, name, price, category };
}

// Eksempeldata
const initialRows = [
  createData(1, "SU", 305, "1"),
  createData(2, "Løn", 452, "2"),
  createData(3, "Sort arbejde", 452, "hej"),
];

export default function IndtægterTabel() {
  const [rows, setRows] = React.useState(initialRows);
  const [anchorEl, setAnchorEl] = React.useState(null);
  const [selected, setSelected] = React.useState([]);
  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(5);

  const handleClickMenu = (event) => {
    setAnchorEl(event.currentTarget); // Åbner menuen ved at sætte anchorEl
  };

  const handleCloseMenu = () => {
    setAnchorEl(null); // Lukker menuen
  };

  const chartData = rows.map((row) => ({
    name: row.name,
    price: row.price,
  }));
  
  const handleAddRow = () => {
    const newRow = createData(
      rows.length + 1,
      prompt("Indtægtsnavn:"),
      prompt("Pris:"),
      prompt("Kategori:")
    );
    setRows([...rows, newRow]); // Tilføjer en ny række
    handleCloseMenu(); // Lukker menuen efter tilføjelsen
  };

  const handleDeleteRow = () => {
    if (selected.length > 0) {
      setRows(rows.filter((row) => !selected.includes(row.id))); // Sletter valgte rækker
      setSelected([]); // Rydder den valgte række
    }
    handleCloseMenu(); // Lukker menuen efter sletning
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

  return (
    <Box sx={{ width: "100%" }}>
      <Paper sx={{ width: "100%", mb: 2 }}>
        <Box sx={{ position: "relative" }}>
          {/* Menu knap */}
          <IconButton
            onClick={handleClickMenu}
            sx={{ position: "absolute", zIndex: 1, top: 10, right: 10 }}
          >
            <MoreVertIcon />
          </IconButton>

          {/* Menuen */}
          <Menu
            anchorEl={anchorEl}
            open={Boolean(anchorEl)} // Menuen vises, hvis anchorEl ikke er null
            onClose={handleCloseMenu} // Lukker menuen, når der klikkes udenfor
          >
            <MenuItem onClick={handleAddRow}>Tilføj indtægt</MenuItem>
            <MenuItem onClick={handleDeleteRow}>Slet indtægt</MenuItem>
            <MenuItem onClick={handleCloseMenu}>Luk menu</MenuItem>
          </Menu>
        </Box>

        {/* Tabel header */}
        <TableContainer>
          <Table sx={{ minWidth: 750 }} aria-labelledby="tableTitle">
            <TableHead>
              <TableRow>
                <TableCell padding="checkbox">
                  <Checkbox
                    color="primary"
                    indeterminate={selected.length > 0 && selected.length < rows.length}
                    checked={rows.length > 0 && selected.length === rows.length}
                    onChange={handleSelectAllClick}
                  />
                </TableCell>
                <TableCell>Indtægtsnavn</TableCell>
                <TableCell align="right">Pris</TableCell>
                <TableCell align="right">Kategori</TableCell>
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
                        <Checkbox
                          color="primary"
                          checked={isItemSelected}
                        />
                      </TableCell>
                      <TableCell>{row.name}</TableCell>
                      <TableCell align="right">{row.price}</TableCell>
                      <TableCell align="right">{row.category}</TableCell>
                    </TableRow>
                  );
                })}
            </TableBody>
          </Table>
        </TableContainer>
        {/* Tabel pagination */}
        <TablePagination
          rowsPerPageOptions={[5, 10, 25]}
          component="div"
          count={rows.length}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </Paper>
      <PieChart chartData={chartData} />
    </Box>
  );
}
