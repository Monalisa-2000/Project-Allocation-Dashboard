import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { allocationsApi } from '@/api/endpoints';
import { toast } from 'sonner';

function resolveApiErrorMessage(err: any, fallback: string): string {
  return err?.response?.data?.message || err?.response?.data?.detail || fallback;
}

export function useAllocations() {
  return useQuery({
    queryKey: ['allocations'],
    queryFn: allocationsApi.getAll,
  });
}

export function useMyAllocations() {
  return useQuery({
    queryKey: ['allocations', 'my'],
    queryFn: allocationsApi.getMy,
  });
}

export function useAssignProjects() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: { userId: string; projectIds: string[] }) => allocationsApi.assign(data),
    onSuccess: (_data, variables) => {
      qc.invalidateQueries({ queryKey: ['allocations'] });
      toast.success('Projects assigned successfully');
    },
    onError: (err: any) => {
      toast.error(resolveApiErrorMessage(err, 'Failed to assign projects'));
    },
  });
}

export function useCompleteAllocation() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => allocationsApi.complete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['allocations'] });
      toast.success('Project marked as complete!');
    },
    onError: (err: any) => {
      toast.error(resolveApiErrorMessage(err, 'Failed to complete allocation'));
    },
  });
}

export function useDeleteAllocation() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => allocationsApi.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['allocations'] });
      toast.success('Allocation removed successfully');
    },
    onError: (err: any) => {
      toast.error(resolveApiErrorMessage(err, 'Failed to remove allocation'));
    },
  });
}
