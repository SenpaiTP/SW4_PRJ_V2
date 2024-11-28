import React from "react";
import {
  TableHead,
  TableRow,
  TableCell,
  Checkbox,
  IconButton,
} from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";

export default function TableHeader({
  numSelected,
  rowCount,
  onSelectAllClick,
  onOpenMenu,
}) {
  return (
    <TableHead>
      <TableRow>
        {/* Checkbox for at vælge alle */}
        <TableCell padding="checkbox">
          <Checkbox
            color="primary"
            indeterminate={numSelected > 0 && numSelected < rowCount}
            checked={rowCount > 0 && numSelected === rowCount}
            onChange={onSelectAllClick}
          />
        </TableCell>

        {/* Kolonnehoveder */}
        <TableCell>Indtægtsnavn</TableCell>
        <TableCell align="right">Beløb</TableCell>
        <TableCell align="right">Dato</TableCell>
        <TableCell align="right"></TableCell>
      </TableRow>
    </TableHead>
  );
}
