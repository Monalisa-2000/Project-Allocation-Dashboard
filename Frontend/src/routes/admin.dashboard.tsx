import { createFileRoute } from '@tanstack/react-router';
import { FolderOpen, Users, ClipboardList, CheckCircle } from 'lucide-react';
import { StatCard } from '@/components/StatCard';
import { useProjects } from '@/hooks/useProjects';
import { useUsers } from '@/hooks/useUsers';
import { useAllocations } from '@/hooks/useAllocations';

export const Route = createFileRoute('/admin/dashboard')({
  component: AdminDashboard,
});

function AdminDashboard() {
  const { data: projects, isLoading: loadingProjects } = useProjects();
  const { data: users, isLoading: loadingUsers } = useUsers();
  const { data: allocations, isLoading: loadingAllocations } = useAllocations();

  const isLoading = loadingProjects || loadingUsers || loadingAllocations;

  const totalProjects = projects?.length ?? 0;
  const totalUsers = users?.length ?? 0;
  const totalAllocations = allocations?.length ?? 0;
  const completionRate = totalAllocations > 0
    ? Math.round((allocations!.filter(a => a.isCompleted).length / totalAllocations) * 100)
    : 0;

  if (isLoading) {
    return (
      <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
        {[1, 2, 3, 4].map(i => (
          <div key={i} className="rounded-lg border bg-card p-6 shadow-sm animate-pulse">
            <div className="h-4 w-1/2 rounded bg-muted mb-3" />
            <div className="h-8 w-1/3 rounded bg-muted" />
          </div>
        ))}
      </div>
    );
  }

  return (
    <div>
      <h1 className="text-2xl font-bold text-foreground mb-6">Overview</h1>
      <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
        <StatCard label="Total Projects" value={totalProjects} icon={FolderOpen} />
        <StatCard label="Total Users" value={totalUsers} icon={Users} />
        <StatCard label="Total Allocations" value={totalAllocations} icon={ClipboardList} />
        <StatCard label="Completion Rate" value={`${completionRate}%`} icon={CheckCircle} color="success" />
      </div>
    </div>
  );
}
