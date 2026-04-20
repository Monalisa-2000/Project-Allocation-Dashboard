import { createFileRoute, useNavigate } from '@tanstack/react-router';
import { useEffect } from 'react';
import { useAuth } from '@/lib/auth';
import { useMyAllocations, useCompleteAllocation } from '@/hooks/useAllocations';
import { ProjectCard } from '@/components/ProjectCard';
import { SkeletonCard } from '@/components/SkeletonCard';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { LogOut, Inbox } from 'lucide-react';

export const Route = createFileRoute('/dashboard')({
  component: UserDashboard,
});

function UserDashboard() {
  const { user, logout, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const { data: allocations = [], isLoading } = useMyAllocations();
  const completeMutation = useCompleteAllocation();

  useEffect(() => {
    if (!isAuthenticated || user?.role !== 'User') {
      navigate({ to: '/login' });
    }
  }, [isAuthenticated, user, navigate]);

  if (!isAuthenticated || user?.role !== 'User') {
    return null;
  }

  const handleLogout = () => {
    logout();
    navigate({ to: '/login' });
  };

  return (
    <div className="min-h-screen">
      {/* Navbar */}
      <header className="sticky top-0 z-30 flex h-14 items-center justify-between border-b bg-card px-6">
        <span className="font-semibold text-foreground">Project Allocation Dashboard</span>
        <div className="flex items-center gap-3">
          <span className="text-sm text-muted-foreground">Hello, <span className="font-medium text-foreground">{user?.name}</span></span>
          <Button variant="ghost" size="icon" onClick={handleLogout} className="text-muted-foreground">
            <LogOut className="h-4 w-4" />
          </Button>
        </div>
      </header>

      <main className="mx-auto max-w-5xl p-6">
        <div className="flex items-center gap-3 mb-6">
          <h1 className="text-2xl font-bold text-foreground">My Projects</h1>
          <Badge variant="secondary">{allocations.length}</Badge>
        </div>

        {isLoading ? (
          <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
            {[1, 2, 3, 4].map(i => <SkeletonCard key={i} />)}
          </div>
        ) : allocations.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-20 text-center">
            <div className="flex h-20 w-20 items-center justify-center rounded-full bg-muted mb-4">
              <Inbox className="h-10 w-10 text-muted-foreground" />
            </div>
            <h2 className="text-lg font-semibold text-foreground">No projects assigned yet</h2>
            <p className="text-sm text-muted-foreground mt-1">When an admin assigns projects to you, they will appear here.</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
            {allocations.map(a => (
              <ProjectCard
                key={a.id}
                allocation={a}
                onComplete={(id) => completeMutation.mutate(id)}
                isCompleting={completeMutation.isPending && completeMutation.variables === a.id}
              />
            ))}
          </div>
        )}
      </main>
    </div>
  );
}
