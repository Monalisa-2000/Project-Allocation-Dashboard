import { Badge } from '@/components/ui/badge';

export function StatusBadge({ isCompleted }: { isCompleted: boolean }) {
  return isCompleted ? (
    <Badge className="bg-success text-success-foreground hover:bg-success/90 border-0">
      Completed
    </Badge>
  ) : (
    <Badge className="bg-warning text-warning-foreground hover:bg-warning/90 border-0">
      Pending
    </Badge>
  );
}
