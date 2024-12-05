import React from "react";
import { TableRow, TableCell } from "@mui/material";

const UdgifterTableBody = ({
  rows,
  selected,
  page,
  rowsPerPage,
  onRowClick,
  renderActions,
}) => {
  const rowsToDisplay = rows.slice(
    page * rowsPerPage,
    page * rowsPerPage + rowsPerPage
  );

  return (
    <>
      {rowsToDisplay.map((row) => (
        <TableRow
          key={row.id}
          selected={selected.indexOf(row.id) !== -1}
          onClick={(e) => onRowClick(e, row.id)}
        >
                 
          <TableCell align="center">{row.name}</TableCell>
          <TableCell align="center">{row.category}</TableCell>
          <TableCell align="center">{row.price}</TableCell>
          <TableCell align="center">{row.date}</TableCell>
          <TableCell>{renderActions(row)}</TableCell>
        </TableRow>
      ))}
    </>
  );
};

export default UdgifterTableBody;