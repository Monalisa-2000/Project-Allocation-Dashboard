import { createFileRoute } from '@tanstack/react-router';
import { useState, useMemo } from 'react';
import {
  useReactTable,
  getCoreRowModel,
  getPaginationRowModel,
  flexRender,
  createColumnHelper,
} from '@tanstack/react-table';
import { Trash2, UserPlus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { SkeletonTable } from '@/components/SkeletonTable';
import { ConfirmDialog } from '@/components/ConfirmDialog';
import { AssignProjectModal } from '@/components/AssignProjectModal';
import { useUsers, useDeleteUser } from '@/hooks/useUsers';
import type { User } from '@/types/models';

export const Route = createFileRoute('/admin/users')({
  component: AdminUsers,
});

const columnHelper = createColumnHelper<User>();

function AdminUsers() {
  const { data: users = [], isLoading } = useUsers();
  const deleteMutation = useDeleteUser();
  const [deleteTarget, setDeleteTarget] = useState<User | null>(null);
  const [assignTarget, setAssignTarget] = useState<User | null>(null);

  const columns = useMemo(() => [
    columnHelper.accessor('name', { header: 'Name', cell: info => <span className="font-medium">{info.getValue()}</span> }),
    columnHelper.accessor('email', { header: 'Email', cell: info => <span className="text-muted-foreground">{info.getValue()}</span> }),
    columnHelper.accessor('createdAt', {
      header: 'Created At',
      cell: info => new Date(info.getValue()).toLocaleDateString(),
    }),
    columnHelper.display({
      id: 'actions',
      header: 'Actions',
      cell: ({ row }) => (
        <div className="flex gap-2">
          <Button variant="outline" size="sm" onClick={() => setAssignTarget(row.original)}>
            <UserPlus className="mr-1 h-3.5 w-3.5" /> Assign Projects
          </Button>
          <Button variant="ghost" size="icon" className="text-destructive hover:text-destructive" onClick={() => setDeleteTarget(row.original)}>
            <Trash2 className="h-4 w-4" />
          </Button>
        </div>
      ),
    }),
  ], []);

  const table = useReactTable({
    data: users,
    columns,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    initialState: { pagination: { pageSize: 10 } },
  });

  if (isLoading) return <SkeletonTable rows={5} cols={4} />;

  return (
    <div>
      <div className="flex items-center gap-3 mb-6">
        <h1 className="text-2xl font-bold text-foreground">Users</h1>
        <Badge variant="secondary">{users.length}</Badge>
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
              <TableRow><TableCell colSpan={columns.length} className="text-center py-8 text-muted-foreground">No users found</TableCell></TableRow>
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

      {assignTarget && (
        <AssignProjectModal
          open={!!assignTarget}
          onOpenChange={() => setAssignTarget(null)}
          userId={assignTarget.id}
          userName={assignTarget.name}
        />
      )}
      <ConfirmDialog
        open={!!deleteTarget}
        onOpenChange={() => setDeleteTarget(null)}
        title="Delete User"
        description="Are you sure you want to delete this user? This action cannot be undone."
        onConfirm={() => { if (deleteTarget) { deleteMutation.mutate(deleteTarget.id); setDeleteTarget(null); } }}
      />
    </div>
  );
}
