import React from "react";
import { TableHead, TableRow, TableCell } from "@mui/material";

export default function TableHeader({ numSelected }) {
  return (
    <TableHead>
      <TableRow>
  
        <TableCell style={{ width: '20%' }}>Udgiftsnavn</TableCell>
        <TableCell style={{ width: '20%' }} align="center">Kategori</TableCell>
        <TableCell style={{ width: '20%' }} align="center">Beløb</TableCell>
        <TableCell style={{ width: '20%' }} align="center">Dato</TableCell>
        <TableCell style={{ width: '20%' }} align="center">Handlinger</TableCell>
      </TableRow>
    </TableHead>
  );
}