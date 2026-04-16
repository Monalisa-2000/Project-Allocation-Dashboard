import { createFileRoute } from '@tanstack/react-router';
import { useState, useMemo } from 'react';
import {
  useReactTable,
  getCoreRowModel,
  getPaginationRowModel,
  getFilteredRowModel,
  flexRender,
  createColumnHelper,
} from '@tanstack/react-table';
import { Trash2, Plus, Search } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { SkeletonTable } from '@/components/SkeletonTable';
import { ConfirmDialog } from '@/components/ConfirmDialog';
import { CreateProjectModal } from '@/components/CreateProjectModal';
import { useProjects, useDeleteProject } from '@/hooks/useProjects';
import type { Project } from '@/types/models';

export const Route = createFileRoute('/admin/projects')({
  component: AdminProjects,
});

const columnHelper = createColumnHelper<Project>();

function AdminProjects() {
  const { data: projects = [], isLoading } = useProjects();
  const deleteMutation = useDeleteProject();
  const [search, setSearch] = useState('');
  const [createOpen, setCreateOpen] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState<Project | null>(null);

  const columns = useMemo(() => [
    columnHelper.accessor('name', { header: 'Name', cell: info => <span className="font-medium">{info.getValue()}</span> }),
    columnHelper.accessor('description', {
      header: 'Description',
      cell: info => {
        const val = info.getValue() || '—';
        return <span className="text-muted-foreground">{val.length > 60 ? val.slice(0, 60) + '…' : val}</span>;
      },
    }),
    columnHelper.accessor('createdAt', {
      header: 'Created At',
      cell: info => new Date(info.getValue()).toLocaleDateString(),
    }),
    columnHelper.display({
      id: 'actions',
      header: 'Actions',
      cell: ({ row }) => (
        <Button variant="ghost" size="icon" className="text-destructive hover:text-destructive" onClick={() => setDeleteTarget(row.original)}>
          <Trash2 className="h-4 w-4" />
        </Button>
      ),
    }),
  ], []);

  const filtered = useMemo(() =>
    projects.filter(p => p.name.toLowerCase().includes(search.toLowerCase())),
    [projects, search]
  );

  const table = useReactTable({
    data: filtered,
    columns,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    initialState: { pagination: { pageSize: 10 } },
  });

  if (isLoading) return <SkeletonTable rows={5} cols={4} />;

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold text-foreground">Projects</h1>
        <Button onClick={() => setCreateOpen(true)}>
          <Plus className="mr-2 h-4 w-4" /> New Project
        </Button>
      </div>

      <div className="relative mb-4 max-w-sm">
        <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
        <Input placeholder="Search projects..." value={search} onChange={e => setSearch(e.target.value)} className="pl-10" />
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
              <TableRow><TableCell colSpan={columns.length} className="text-center py-8 text-muted-foreground">No projects found</TableCell></TableRow>
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
        <p className="text-sm text-muted-foreground">
          Page {table.getState().pagination.pageIndex + 1} of {table.getPageCount() || 1}
        </p>
        <div className="flex gap-2">
          <Button variant="outline" size="sm" onClick={() => table.previousPage()} disabled={!table.getCanPreviousPage()}>Previous</Button>
          <Button variant="outline" size="sm" onClick={() => table.nextPage()} disabled={!table.getCanNextPage()}>Next</Button>
        </div>
      </div>

      <CreateProjectModal open={createOpen} onOpenChange={setCreateOpen} />
      <ConfirmDialog
        open={!!deleteTarget}
        onOpenChange={() => setDeleteTarget(null)}
        title="Delete Project"
        description="Are you sure you want to delete this project? All allocations will also be removed."
        onConfirm={() => { if (deleteTarget) { deleteMutation.mutate(deleteTarget.id); setDeleteTarget(null); } }}
      />
    </div>
  );
}
