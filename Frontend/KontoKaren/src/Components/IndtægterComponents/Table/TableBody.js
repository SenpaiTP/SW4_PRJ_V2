// Inside TableBody.js
import React from 'react';
import { TableRow, TableCell } from '@mui/material';

const TableBody = ({ rows, selected, page, rowsPerPage, onRowClick, renderActions }) => {
  const rowsToDisplay = rows.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);

  return (
    <>
      {rowsToDisplay.map((row) => (
        <TableRow
          key={row.id}
          selected={selected.indexOf(row.id) !== -1}
          onClick={(e) => onRowClick(e, row.id)}
        >
          <TableCell padding="checkbox">
            <input
              type="checkbox"
              checked={selected.indexOf(row.id) !== -1}
              onChange={(e) => onRowClick(e, row.id)}
            />
          </TableCell>
          <TableCell>{row.name}</TableCell>
          <TableCell>{row.price}</TableCell>
          <TableCell>{row.date}</TableCell>
          {renderActions(row)} {/* Render the action buttons */}
        </TableRow>
      ))}
    </>
  );
};

export default TableBody;
