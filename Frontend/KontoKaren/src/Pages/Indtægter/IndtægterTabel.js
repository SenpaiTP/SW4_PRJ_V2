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

// Funktion til at oprette data
function createData(id, name, price, category) {
  return {
    id,
    name,
    price,
    category,
  };
}

// Eksempeldata
const initialRows = [
  createData(1, "SU", 305, "1"),
  createData(2, "Løn", 452, "2"),
  createData(3, "Sort arbejde", 452, "hej"),
];

// Helper-funktioner til at sortere tabel
function descendingComparator(a, b, orderBy) {
  if (b[orderBy] < a[orderBy]) {
    return -1;
  }
  if (b[orderBy] > a[orderBy]) {
    return 1;
  }
  return 0;
}

function getComparator(order, orderBy) {
  return order === "desc"
    ? (a, b) => descendingComparator(a, b, orderBy)
    : (a, b) => -descendingComparator(a, b, orderBy);
}

// Hoved celler i tabellen
const headCells = [
  {
    id: "name",
    numeric: false,
    disablePadding: true,
    label: "Indtægter",
  },
  {
    id: "price",
    numeric: true,
    disablePadding: false,
    label: "Pris",
  },
  {
    id: "category",
    numeric: true,
    disablePadding: false,
    label: "Kategori",
  },
];

// Tabel Header komponent
function EnhancedTableHead(props) {
  const {
    onSelectAllClick,
    order,
    orderBy,
    numSelected,
    rowCount,
    onRequestSort,
  } = props;
  const createSortHandler = (property) => (event) => {
    onRequestSort(event, property);
  };

  return (
    <TableHead>
      <TableRow>
        <TableCell padding="checkbox">
          <Checkbox
            color="primary"
            indeterminate={numSelected > 0 && numSelected < rowCount}
            checked={rowCount > 0 && numSelected === rowCount}
            onChange={onSelectAllClick}
            inputProps={{
              "aria-label": "select all desserts",
            }}
          />
        </TableCell>
        {headCells.map((headCell) => (
          <TableCell
            key={headCell.id}
            align={headCell.numeric ? "right" : "left"}
            padding={headCell.disablePadding ? "none" : "normal"}
            sortDirection={orderBy === headCell.id ? order : false}
          >
            <TableSortLabel
              active={orderBy === headCell.id}
              direction={orderBy === headCell.id ? order : "asc"}
              onClick={createSortHandler(headCell.id)}
            >
              {headCell.label}
              {orderBy === headCell.id ? (
                <Box component="span" sx={visuallyHidden}>
                  {order === "desc" ? "sorted descending" : "sorted ascending"}
                </Box>
              ) : null}
            </TableSortLabel>
          </TableCell>
        ))}
      </TableRow>
    </TableHead>
  );
}

EnhancedTableHead.propTypes = {
  numSelected: PropTypes.number.isRequired,
  onRequestSort: PropTypes.func.isRequired,
  onSelectAllClick: PropTypes.func.isRequired,
  order: PropTypes.oneOf(["asc", "desc"]).isRequired,
  orderBy: PropTypes.string.isRequired,
  rowCount: PropTypes.number.isRequired,
};

function EnhancedTableToolbar(props) {
  const { numSelected } = props;
  return (
    <Toolbar
      sx={[
        {
          pl: { sm: 2 },
          pr: { xs: 1, sm: 1 },
        },
        numSelected > 0 && {
          bgcolor: (theme) =>
            alpha(
              theme.palette.primary.main,
              theme.palette.action.activatedOpacity
            ),
        },
      ]}
    >
      {numSelected > 0 ? (
        <Typography
          sx={{ flex: "1 1 100%" }}
          color="inherit"
          variant="subtitle1"
          component="div"
        >
          {numSelected} selected
        </Typography>
      ) : (
        <Typography
          sx={{ flex: "1 1 100%" }}
          variant="h6"
          id="tableTitle"
          component="div"
        >
          Indtast dine indtægter
        </Typography>
      )}
    </Toolbar>
  );
}

EnhancedTableToolbar.propTypes = {
  numSelected: PropTypes.number.isRequired,
};

export default function IndtægterTabel() {
    const [order, setOrder] = React.useState("asc");
    const [orderBy, setOrderBy] = React.useState("price");
    const [selected, setSelected] = React.useState([]);
    const [page, setPage] = React.useState(0);
    const [dense, setDense] = React.useState(false);
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [rows, setRows] = React.useState(initialRows);
    const [anchorEl, setAnchorEl] = React.useState(null);
  
    const emptyRows =
      page > 0 ? Math.max(0, (1 + page) * rowsPerPage - rows.length) : 0;
  
    const handleRequestSort = (event, property) => {
      const isAsc = orderBy === property && order === "asc";
      setOrder(isAsc ? "desc" : "asc");
      setOrderBy(property);
    };
  
    const handleSelectAllClick = (event) => {
      if (event.target.checked) {
        const newSelected = rows.map((n) => n.id);
        setSelected(newSelected);
        return;
      }
      setSelected([]);
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
  
    const handleChangeDense = (event) => {
      setDense(event.target.checked);
    };
  
    const handleClickMenu = (event) => {
      setAnchorEl(event.currentTarget);
    };
  
    const handleCloseMenu = () => {
      setAnchorEl(null);
    };
  
    const handleAddRow = () => {
      const newRow = createData(
        rows.length + 1,
        prompt("Indtægtsnavn:"),
        prompt("Pris:"),
        prompt("Kategori:")
      );
      setRows([...rows, newRow]);
      handleCloseMenu();
    };
  
    const handleEditRow = () => {
      const updatedRows = rows.map((row) =>
        row.id === selected[0] // Use the first selected row
          ? {
              ...row,
              name: prompt("Indtægtsnavn:", row.name),
              price: prompt("Pris:", row.price),
              category: prompt("Kategori:", row.category),
            }
          : row
      );
      setRows(updatedRows);
      handleCloseMenu();
    };
  
    const handleDeleteRow = () => {
      setRows(rows.filter((row) => row.id !== selected[0])); // Use the first selected row
      setSelected([]);
      handleCloseMenu();
    };
  
    return (
      <Box sx={{ width: "100%" }}>
        <Paper sx={{ width: "100%", mb: 2 }}>
          <Box sx={{ position: "relative" }}>
            <IconButton
              onClick={handleClickMenu}
              sx={{ position: "absolute", zIndex: 1, top: 10, right: 10 }}
              >
              <MoreVertIcon />
            </IconButton>
  
            <Menu
              anchorEl={anchorEl}
              open={Boolean(anchorEl)}
              onClose={handleCloseMenu}
            >
              <MenuItem onClick={handleAddRow}>Tilføj</MenuItem>
              <MenuItem onClick={handleEditRow}>Rediger</MenuItem>
              <MenuItem onClick={handleDeleteRow}>Slet</MenuItem>
            </Menu>
          </Box>
  
          <EnhancedTableToolbar numSelected={selected.length} />
      
          <TableContainer>
            <Table sx={{ minWidth: 750 }} aria-labelledby="tableTitle" size={dense ? "small" : "medium"}>
              <EnhancedTableHead
                numSelected={selected.length}
                order={order}
                orderBy={orderBy}
                onSelectAllClick={handleSelectAllClick}
                onRequestSort={handleRequestSort}
                rowCount={rows.length}
              />
              <TableBody>
                {rows
                  .sort(getComparator(order, orderBy))
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
                            inputProps={{
                              "aria-labelledby": row.id,
                            }}
                          />
                        </TableCell>
                        <TableCell component="th" id={row.id} scope="row" padding="none">
                          {row.name}
                        </TableCell>
                        <TableCell align="right">{row.price}</TableCell>
                        <TableCell align="right">{row.category}</TableCell>
                      </TableRow>
                    );
                  })}
                {emptyRows > 0 && (
                  <TableRow
                    style={{
                      height: (dense ? 33 : 53) * emptyRows,
                    }}
                  >
                    <TableCell colSpan={6} />
                  </TableRow>
                )}
              </TableBody>
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
          />
        </Paper>
      </Box>
    );
  }
  