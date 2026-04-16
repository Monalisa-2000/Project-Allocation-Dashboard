import { createFileRoute } from '@tanstack/react-router';
import { useState, useMemo } from 'react';
import {
  useReactTable,
  getCoreRowModel,
  getPaginationRowModel,
  flexRender,
  createColumnHelper,
} from '@tanstack/react-table';
import { X, Search } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Badge } from '@/components/ui/badge';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { StatusBadge } from '@/components/StatusBadge';
import { SkeletonTable } from '@/components/SkeletonTable';
import { ConfirmDialog } from '@/components/ConfirmDialog';
import { useAllocations, useDeleteAllocation } from '@/hooks/useAllocations';
import type { Allocation } from '@/types/models';

export const Route = createFileRoute('/admin/allocations')({
  component: AdminAllocations,
});

const columnHelper = createColumnHelper<Allocation>();

function AdminAllocations() {
  const { data: allocations = [], isLoading } = useAllocations();
  const deleteMutation = useDeleteAllocation();
  const [userFilter, setUserFilter] = useState('');
  const [projectFilter, setProjectFilter] = useState('');
  const [deleteTarget, setDeleteTarget] = useState<Allocation | null>(null);

  const filtered = useMemo(() =>
    allocations.filter(a =>
      a.userName.toLowerCase().includes(userFilter.toLowerCase()) &&
      a.projectName.toLowerCase().includes(projectFilter.toLowerCase())
    ), [allocations, userFilter, projectFilter]);

  const columns = useMemo(() => [
    columnHelper.accessor('userName', { header: 'User', cell: info => <span className="font-medium">{info.getValue()}</span> }),
    columnHelper.accessor('projectName', { header: 'Project', cell: info => info.getValue() }),
    columnHelper.accessor('assignedAt', {
      header: 'Assigned At',
      cell: info => new Date(info.getValue()).toLocaleDateString(),
    }),
    columnHelper.accessor('isCompleted', {
      header: 'Status',
      cell: info => <StatusBadge isCompleted={info.getValue()} />,
    }),
    columnHelper.accessor('completedAt', {
      header: 'Completed At',
      cell: info => info.getValue() ? new Date(info.getValue()!).toLocaleDateString() : '—',
    }),
    columnHelper.display({
      id: 'actions',
      header: 'Actions',
      cell: ({ row }) => (
        <Button variant="ghost" size="icon" className="text-destructive hover:text-destructive" onClick={() => setDeleteTarget(row.original)}>
          <X className="h-4 w-4" />
        </Button>
      ),
    }),
  ], []);

  const table = useReactTable({
    data: filtered,
    columns,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    initialState: { pagination: { pageSize: 10 } },
  });

  if (isLoading) return <SkeletonTable rows={5} cols={6} />;

  return (
    <div>
      <div className="flex items-center gap-3 mb-6">
        <h1 className="text-2xl font-bold text-foreground">Allocations</h1>
        <Badge variant="secondary">{allocations.length}</Badge>
      </div>

      <div className="flex flex-col sm:flex-row gap-3 mb-4">
        <div className="relative max-w-xs flex-1">
          <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <Input placeholder="Filter by user..." value={userFilter} onChange={e => setUserFilter(e.target.value)} className="pl-10" />
        </div>
        <div className="relative max-w-xs flex-1">
          <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <Input placeholder="Filter by project..." value={projectFilter} onChange={e => setProjectFilter(e.target.value)} className="pl-10" />
        </div>
      </div>

      <div className="rounded-lg border bg-card shadow-sm overflow-hidden">
        <Table>
          <TableHeader>
            {table.getHeaderGroups().map(hg => (
              <TableRow key={hg.id}>
                {hg.headers.map(h => (
                  <TableHead key={h.id}>{h.isPlaceholder ? null : flexRender(h.column.columnDef.header, h.getContext())}</TableHead>
                ))}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {table.getRowModel().rows.length === 0 ? (
              <TableRow><TableCell colSpan={columns.length} className="text-center py-8 text-muted-foreground">No allocations found</TableCell></TableRow>
            ) : (
              table.getRowModel().rows.map(row => (
                <TableRow key={row.id}>
                  {row.getVisibleCells().map(cell => (
                    <TableCell key={cell.id}>{flexRender(cell.column.columnDef.cell, cell.getContext())}</TableCell>
                  ))}
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </div>

      <div className="flex items-center justify-between mt-4">
        <p className="text-sm text-muted-foreground">Page {table.getState().pagination.pageIndex + 1} of {table.getPageCount() || 1}</p>
        <div className="flex gap-2">
          <Button variant="outline" size="sm" onClick={() => table.previousPage()} disabled={!table.getCanPreviousPage()}>Previous</Button>
          <Button variant="outline" size="sm" onClick={() => table.nextPage()} disabled={!table.getCanNextPage()}>Next</Button>
        </div>
      </div>

      <ConfirmDialog
        open={!!deleteTarget}
        onOpenChange={() => setDeleteTarget(null)}
        title="Unassign Allocation"
        description="Are you sure you want to remove this allocation?"
        onConfirm={() => { if (deleteTarget) { deleteMutation.mutate(deleteTarget.id); setDeleteTarget(null); } }}
      />
    </div>
  );
}
