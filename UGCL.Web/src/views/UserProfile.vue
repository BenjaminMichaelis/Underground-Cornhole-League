<template>
  <v-container max-width="1000px">
    <v-card>
      <v-card-title>User Profile</v-card-title>
      <c-loader-status
        :loaders="{
          'no-initial-content no-error-content': [user.$load],
          //'': [user.$bulkSave],
        }"
      >
        <v-card-text>
          <c-input :model="user" for="userName"></c-input>
          <c-input :model="user" for="fullName"></c-input>

          <div class="d-flex">
            <c-input
              :model="user"
              for="email"
              readonly
              :hint="
                user.emailConfirmed ? 'Email Verified' : 'Email not verified'
              "
              persistent-hint
              class="mb-3"
            >
              <template #append-inner>
                <v-icon v-if="user.emailConfirmed" color="success">
                  fa fa-check-circle
                </v-icon>
                <v-icon v-else color="warning">
                  fa fa-exclamation-triangle
                </v-icon>
              </template>
            </c-input>
          </div>
        </v-card-text>

        <template v-if="isUserAdmin">
          <v-card-title> Roles & permissions </v-card-title>
          <v-card-text>
            <v-row>
              <v-col>
                <c-input :model="user" for="userRoles"></c-input>
              </v-col>
              <v-col>
                <h3>Effective Permissions:</h3>
                <div style="max-height: 300px; overflow-y: auto">
                  <div
                    v-for="permission in PermissionMeta.values.map((p) => ({
                      meta: p,
                      roles: user.roles.filter((r) =>
                        r.permissions?.includes(p.value),
                      ),
                    }))"
                    :key="permission.meta.strValue"
                  >
                    <span v-if="permission.roles.length">
                      <v-icon icon="fa fa-check text-success"></v-icon>
                      {{ permission.meta.displayName }}
                      <span class="text-caption text-grey pl-1">
                        (via
                        {{ permission.roles.map((r) => r.name).join(",") }})
                      </span>
                    </span>
                    <span v-else>
                      <v-icon icon="fa fa-times text-error"></v-icon>
                      {{ permission.meta.displayName }}
                    </span>
                  </div>
                </div>
              </v-col>
            </v-row>
          </v-card-text>
        </template>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="success"
            prepend-icon="fa fa-save"
            :loading="user.$bulkSave.isLoading"
            :disabled="!user.$bulkSavePreview().isDirty"
            :variant="!user.$bulkSavePreview().isDirty ? 'text' : 'elevated'"
            @click="user.$bulkSave()"
          >
            Save
          </v-btn>
        </v-card-actions>
      </c-loader-status>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { UserViewModel } from "@/viewmodels.g";
import { Permission } from "@/models.g";
import { Permission as PermissionMeta } from "@/metadata.g";

const router = useRouter();
const { can, userInfo } = useUser();

const props = defineProps<{ id: string }>();
const user = new UserViewModel();

const isUserAdmin = computed(() => can(Permission.UserAdmin));

const isMe = computed(() => props.id == userInfo.value.id);

if (!isUserAdmin.value && !isMe.value) {
  // Non-admins can only view themselves
  router.replace({ name: "error-404" });
} else {
  user.$load(props.id);
}
</script>
