import { useState, useEffect } from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Checkbox } from '@/components/ui/checkbox';
import { Label } from '@/components/ui/label';
import { Loader2 } from 'lucide-react';
import { useProjects } from '@/hooks/useProjects';
import { useAllocations, useAssignProjects } from '@/hooks/useAllocations';

interface AssignProjectModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  userId: string;
  userName: string;
}

export function AssignProjectModal({ open, onOpenChange, userId, userName }: AssignProjectModalProps) {
  const { data: projects = [] } = useProjects();
  const { data: allocations = [] } = useAllocations();
  const assignMutation = useAssignProjects();
  const [selected, setSelected] = useState<string[]>([]);

  const assignedProjectIds = allocations
    .filter(a => a.userId === userId)
    .map(a => a.projectId);

  useEffect(() => {
    if (open) setSelected([]);
  }, [open]);

  const handleSubmit = () => {
    if (selected.length === 0) return;
    assignMutation.mutate(
      { userId, projectIds: selected },
      {
        onSuccess: () => {
          onOpenChange(false);
        },
      }
    );
  };

  const toggleProject = (projectId: string) => {
    setSelected(prev =>
      prev.includes(projectId) ? prev.filter(id => id !== projectId) : [...prev, projectId]
    );
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-md">
        <DialogHeader>
          <DialogTitle>Assign Projects to {userName}</DialogTitle>
        </DialogHeader>
        <div className="max-h-64 overflow-y-auto space-y-3 py-4">
          {projects.map(project => {
            const isAssigned = assignedProjectIds.includes(project.id);
            return (
              <div key={project.id} className="flex items-center gap-3">
                <Checkbox
                  id={`proj-${project.id}`}
                  checked={isAssigned || selected.includes(project.id)}
                  disabled={isAssigned}
                  onCheckedChange={() => !isAssigned && toggleProject(project.id)}
                />
                <Label htmlFor={`proj-${project.id}`} className={isAssigned ? 'text-muted-foreground' : ''}>
                  {project.name}
                  {isAssigned && <span className="ml-2 text-xs text-muted-foreground">(Already Assigned)</span>}
                </Label>
              </div>
            );
          })}
          {projects.length === 0 && (
            <p className="text-sm text-muted-foreground text-center py-4">No projects available</p>
          )}
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>Cancel</Button>
          <Button onClick={handleSubmit} disabled={selected.length === 0 || assignMutation.isPending}>
            {assignMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
            Assign ({selected.length})
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
