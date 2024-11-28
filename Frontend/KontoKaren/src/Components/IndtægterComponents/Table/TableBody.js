import React from "react";
import { TableRow, TableCell, Checkbox } from "@mui/material";

export default function TableBody({
  rows,
  selected,
  page,
  rowsPerPage,
  onRowClick,
}) {
  return (
    <>
      {rows.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row) => {
        const isSelected = selected.includes(row.id);
        return (
          <TableRow
            hover
            onClick={(event) => onRowClick(event, row.id)}
            role="checkbox"
            aria-checked={isSelected}
            tabIndex={-1}
            key={row.id}
            selected={isSelected}
          >
            <TableCell padding="checkbox">
              <Checkbox color="primary" checked={isSelected} />
            </TableCell>
            <TableCell>{row.name}</TableCell>
            <TableCell align="right">{row.price}</TableCell>
            <TableCell align="right">
              {new Date(row.date).toLocaleDateString("da-DK")}
            </TableCell>
          </TableRow>
        );
      })}
    </>
  );
}
