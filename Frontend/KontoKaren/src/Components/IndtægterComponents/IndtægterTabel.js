import React, { useState } from "react";
import { Box, Paper, Table, TableContainer, IconButton } from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";

import TableBody from "./Table/TableBody";
import TableHeader from "./PieChart/TableHeader";
import TableActionMenu from "./Table/TableActionMenu";
import TablePage from "./Table/TablePage";
import PieChart from "./PieChart/PieChart";
import { createData, initialRows } from "./Table/TableData";

// useIntægterHooks hooken
function useIntægterHooks(initialRows) {
  const [rows, setRows] = useState(initialRows);
  const [anchorEl, setAnchorEl] = useState(null);
  const [selected, setSelected] = useState([]);
  const [selectedRow, setSelectedRow] = useState(null);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(5);

  const chartData = rows.map((row) => ({
    name: row.name,
    price: row.price,
  }));

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
      setSelected(rows.map((n) => n.id)); // Vælg alle rækker
    } else {
      setSelected([]); // Fjern markeringen af alle rækker
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
    setSelected(newSelected); // Opdaterer de valgte rækker
  };

  const handleChangePage = (event, newPage) => {
    setPage(newPage); // Opdaterer den aktuelle side i pagineringen
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10)); // Ændrer antallet af rækker pr. side
    setPage(0); // Nulstiller siden til 0
  };

  return {
    rows,
    setRows,
    anchorEl,
    setAnchorEl,
    selected,
    setSelected,
    selectedRow,
    setSelectedRow,
    page,
    setPage,
    rowsPerPage,
    setRowsPerPage,
    handleSelectAllClick,
    handleClick,
    handleChangePage,
    handleChangeRowsPerPage,
    handleAddRow,
    handleEditRow,
    handleDeleteRow,
  };
}

// IndtægterTabel komponenten
export default function IndtægterTabel() {
  const {
    rows,
    setRows,
    anchorEl,
    setAnchorEl,
    selected,
    setSelected,
    selectedRow,
    setSelectedRow,
    page,
    setPage,
    rowsPerPage,
    setRowsPerPage,
    handleSelectAllClick,
    handleClick,
    handleChangePage,
    handleChangeRowsPerPage,
    handleAddRow,
    handleEditRow,
    handleDeleteRow,
  } = useIntægterHooks(initialRows);

  const chartData = rows.map((row) => ({
    name: row.name,
    price: row.price,
  }));

  const handleClickMenu = (event, row) => {
    setAnchorEl(event.currentTarget); // Åbner menuen
    setSelectedRow(row); // Gem den række, der er knyttet til denne menu
  };

  const handleCloseMenu = () => {
    setAnchorEl(null); // Lukker menuen
    setSelectedRow(null); // Rydder valgt række
  };

  return (
    <Box sx={{ width: "100%" }}>
      <Paper sx={{ width: "100%", mb: 2, position: "relative" }}>
        <IconButton
          onClick={(e) => setAnchorEl(e.currentTarget)}
          sx={{ position: "absolute", zIndex: 1, top: 10, right: 10 }}
        >
          <MoreVertIcon />
        </IconButton>

        <TableContainer>
          <Table>
            <TableHeader
              numSelected={selected.length}
              rowCount={rows.length}
              onSelectAllClick={handleSelectAllClick}
            />
            <TableBody
              rows={rows}
              selected={selected}
              page={page}
              rowsPerPage={rowsPerPage}
              onRowClick={handleClick}
            />
          </Table>
        </TableContainer>

        <TableActionMenu
          anchorEl={anchorEl}
          onCloseMenu={handleCloseMenu}
          onAddRow={handleAddRow}
          onEditRow={handleEditRow}
          onDeleteRow={handleDeleteRow}
        />

        <TablePage
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
