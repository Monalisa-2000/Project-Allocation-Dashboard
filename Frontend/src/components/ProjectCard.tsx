import { CheckCircle, Loader2 } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { StatusBadge } from '@/components/StatusBadge';
import type { Allocation } from '@/types/models';

interface ProjectCardProps {
  allocation: Allocation;
  onComplete: (id: string) => void;
  isCompleting: boolean;
}

export function ProjectCard({ allocation, onComplete, isCompleting }: ProjectCardProps) {
  const borderColor = allocation.isCompleted ? 'border-success' : 'border-primary';

  return (
    <div className={`rounded-lg border-2 ${borderColor} bg-card p-6 shadow-sm flex flex-col`}>
      <div className="flex items-start justify-between mb-2">
        <h3 className="text-lg font-semibold text-card-foreground">{allocation.projectName}</h3>
        <StatusBadge isCompleted={allocation.isCompleted} />
      </div>
      <p className="text-sm text-muted-foreground line-clamp-2 mb-3 flex-1">
        {allocation.projectName}
      </p>
      <p className="text-xs text-muted-foreground mb-4">
        Assigned: {new Date(allocation.assignedAt).toLocaleDateString()}
      </p>
      {allocation.isCompleted ? (
        <Button disabled variant="secondary" className="w-full">
          <CheckCircle className="mr-2 h-4 w-4" /> Completed ✓
        </Button>
      ) : (
        <Button
          onClick={() => onComplete(allocation.id)}
          disabled={isCompleting}
          className="w-full bg-success text-success-foreground hover:bg-success/90"
        >
          {isCompleting ? (
            <Loader2 className="mr-2 h-4 w-4 animate-spin" />
          ) : (
            <CheckCircle className="mr-2 h-4 w-4" />
          )}
          Mark Complete
        </Button>
      )}
    </div>
  );
}
