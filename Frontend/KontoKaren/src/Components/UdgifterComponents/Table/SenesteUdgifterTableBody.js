// Inside TableBody.js
import React from "react";
import { TableRow, TableCell } from "@mui/material";

const SenesteTableBody = ({
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
          <TableCell>{}</TableCell>
          <TableCell>{row.name}</TableCell>
          <TableCell>{row.category}</TableCell>
          <TableCell>{row.price}</TableCell>
          <TableCell>{row.date}</TableCell>
          {renderActions(row)} {/* Render the action buttons */}
        </TableRow>
      ))}
    </>
  );
};

export default SenesteTableBody;
